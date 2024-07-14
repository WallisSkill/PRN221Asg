using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services;
using PRN221_Assignment.Services.Interface;
using System.IO;
using System.Linq;

namespace PRN221_Assignment.Pages
{
    [Authorize]
    [RequiredArgsConstructor]

    public partial class EditPostModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int PostId { get; set; }
        public Post post { get; set; }
        private readonly IPostService _postService;
        private readonly IPhotoService _photoService;

        public IActionResult OnGet(int PostId)
        {
            ViewData["post"] = _postService.GetPostById(PostId);
            ViewData["photo"] = _photoService.GetPhotosById(PostId);

            return Page();
        }
        
        public async Task<IActionResult> OnPost(Post post, string imageNames, List<IFormFile> listfile)
        {
            ViewData["post"] = _postService.GetPostById(post.PostId);
            ViewData["photo"] = _photoService.GetPhotosById(post.PostId);
            var listphoto = _photoService.GetPhotosById(post.PostId);
            var listPhotoName = new List<string>();

            if (imageNames != null && listphoto.Count != 0)
            {
                var listPhotoUrlExist = imageNames.Split("||");
                //_photoService.DeletePhoto(post.PostId);
                foreach (var photoUrlDb in listphoto)
                {
                    var a = listPhotoUrlExist.Contains(photoUrlDb.PhotoUrl);
                    if (a)
                    {
                        listPhotoName.Add(photoUrlDb.PhotoUrl.Split('/').Last());
                    }
                    else
                    {
                        var nameFile = photoUrlDb.PhotoUrl.Split('/').Last();
                        string filePathExist = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", nameFile);
                        if (System.IO.File.Exists(filePathExist))
                        {
                            System.IO.File.Delete(filePathExist);
                        }
                    }
                }
            }
            else if (imageNames == null && listphoto.Count != 0)
            {
                foreach (var photo in listphoto)
                {
                    var name = photo.PhotoUrl.Split('/').Last();
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", name);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            if (listfile.Count != 0)
            {
                foreach (var file in listfile)
                {
                    if (file.Length > 0)
                    {
                        string fileExtension = Path.GetExtension(file.FileName);
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                        string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic");
                        string filePath1 = Path.Combine(directory, file.FileName);
                        int index = 1;

                        
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        
                        while (System.IO.File.Exists(filePath1))
                        {
                            filePath1 = Path.Combine(directory, fileNameWithoutExtension + "(" + index + ")" + fileExtension);
                            index++;
                        }


                        try
                        {
                            using (var stream = new FileStream(filePath1, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            // Handle exception
                        }

                        listPhotoName.Add(Path.GetFileName(filePath1));
                    }
                }
            }


            
                _photoService.DeletePhoto(post.PostId);
            
                

            var p = _postService.GetPostById(post.PostId);
            if (p.Caption != post.Caption)
            {
                _postService.UpdatePost(post);
            }
            if (listPhotoName.Count != 0)
            {
                foreach (string photoName in listPhotoName)
                {
                    Photo photo = new Photo();
                    photo.PhotoUrl = "/Image/UploadPic/" + photoName;
                    photo.CreatedAt = DateTime.Now;
                    photo.PostId = post.PostId;
                    _photoService.AddPhoto(photo);
                }
            }


            return RedirectToPage("/Profile");
        }

       
    }
}
