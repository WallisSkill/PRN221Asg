using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages;

[Authorize]
[RequiredArgsConstructor]
public partial class Profile : PageModel
{
    private readonly IProfileService _profileService;
    private readonly IUserResolverService _userResolver;
    private readonly IPostService _postService;
    private readonly IPhotoService _photoService;
    private int? _id;
    [BindProperty(SupportsGet = true)]
    public int Id
    {
        get => _id ?? _userResolver.GetUser(); 
        set => _id = value;
    }
    public Post post { get; set; }
    public IActionResult OnGet()
    {
        if (Id == 0) return RedirectToPage("/Index");
        ViewData["User"] = _profileService.GetUserInfo(Id);
        ViewData["photos"] = _profileService.GetUserPhoto(Id);
        ViewData["likes"] = _profileService.GetCountNumberLikes(Id);
        ViewData["comments"] = _profileService.GetCountNumberComments(Id);
        var b = _profileService.GetAllFriendOfUser(Id);
        ViewData["friends"] = b;
        var a =  _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
        ViewData["userFriends"] = a;
        ViewData["listPost"] = _postService.GetAllPostOfFriendAndFollower(Id);
        ViewData["listSaved"] = _postService.GetAllPostIdsaved();
        return Page();
    }

    public IActionResult OnPostDeletePost(int postId)
    {
        try
        {
            _postService.DeletePost(postId);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
           
            return new JsonResult(new { success = false, error = "Failed to delete post." });
        }
    }

    //Create post in profile
    public async Task<IActionResult> OnPost(Post post, List<IFormFile> listfile)
    {
        List<string> listPhotoName = new List<string>();
        if (listfile != null)
        {
            foreach (var file in listfile)
            {
                if (file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", file.FileName);
                    int index = 1;

                    while (System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", fileNameWithoutExtension + "(" + index + ")" + fileExtension);
                        index++;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    listPhotoName.Add(Path.GetFileName(filePath));
                }
            }
        }

        post.CreatedAt = DateTime.Now;

        _postService.CreatePost(post);

        foreach (string photoName in listPhotoName)
        {
            Photo photo = new Photo();
            photo.PhotoUrl = "/Image/UploadPic/" + photoName;
            photo.CreatedAt = DateTime.Now;
            photo.PostId = post.PostId;
            _photoService.AddPhoto(photo);
        }
        var newPostData = new
        {
            post,
            Photos = listPhotoName.Select(photoName => "/Image/UploadPic/" + photoName).ToList()
        };

        return new JsonResult(newPostData);
    }

    public async Task<IActionResult> OnPostUpdatePost(Post post,List<IFormFile> listfile)
    {
        List<string> listPhotoName = new List<string>();
        if (listfile != null)
        {
            foreach (var file in listfile)
            {
                if (file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", file.FileName);
                    int index = 1;

                    while (System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", fileNameWithoutExtension + "(" + index + ")" + fileExtension);
                        index++;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    listPhotoName.Add(Path.GetFileName(filePath));
                }
            }
        }

        _postService.UpdatePost(post);
        _photoService.DeletePhoto(post.PostId);
        foreach (string photoName in listPhotoName)
        {
            Photo photo = new Photo();
            photo.PhotoUrl = "/Image/UploadPic/" + photoName;
            photo.CreatedAt = DateTime.Now;
            photo.PostId = post.PostId;
            
            _photoService.AddPhoto(photo);
        }
        var newPostData = new
        {
            post,
            Photos = listPhotoName.Select(photoName => "/Image/UploadPic/" + photoName).ToList()
        };


        return new JsonResult(new { success = true });
    }
}