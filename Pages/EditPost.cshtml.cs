using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services;
using PRN221_Assignment.Services.Interface;
using System.Linq;

namespace PRN221_Assignment.Pages
{
    [Authorize]
    [RequiredArgsConstructor]

    public partial class EditPostModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int postId { get; set; }
        public Post post { get; set; }
        private readonly IPostService _postService;
        private readonly IPhotoService _photoService;

        public IActionResult OnGet()
        {
            ViewData["post"] = _postService.GetPostById(postId);
            ViewData["photo"] = _photoService.GetPhotosById(postId);

            return Page();
        }
        public IActionResult OnGetEdit()
        {
            ViewData["post"] = _postService.GetPostById(postId);
            ViewData["photo"] = _photoService.GetPhotosById(postId);

            return Page();
        }

        public async Task<IActionResult> OnPost(Post post, string imageNames, List<IFormFile> uploadedFiles)
        {
            //if (uploadedFiles.Count != 0)
            //{
            //    var listPhotoName = imageNames.Split("||");
            //    var listNamekhongtrungfilename = new List<string>();

            //    var fileNames = new List<string>(uploadedFiles.Select(f => f.FileName));

            //    foreach (var name in fileNames)
            //    {
            //        foreach (var s in listPhotoName)
            //        {

            //            if (!name.Contains(s.Split('/').Last()))
            //            {
            //                listNamekhongtrungfilename.Add(s.Split('/').Last());
            //            }
            //        }
            //    }
            //    _photoService.DeletePhoto(post.PostId);

            //    foreach (var file in uploadedFiles)
            //    {
            //        if (file.Length > 0)
            //        {
            //            string fileExtension = Path.GetExtension(file.FileName);
            //            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            //            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", file.FileName);
            //            int index = 1;

            //            while (System.IO.File.Exists(filePath))
            //            {
            //                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", fileNameWithoutExtension + "(" + index + ")" + fileExtension);
            //                index++;
            //            }

            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                await file.CopyToAsync(stream);
            //            }
            //            listNamekhongtrungfilename.Add(Path.GetFileName(filePath));
            //        }
            //    }


            //    foreach (string photoName in listNamekhongtrungfilename)
            //    {
            //        Photo photo = new Photo();
            //        photo.PhotoUrl = "/Image/UploadPic/" + photoName;
            //        photo.CreatedAt = DateTime.Now;
            //        photo.PostId = post.PostId;
            //        _photoService.AddPhoto(photo);
            //    }
            //}
            //if (imageNames != null && uploadedFiles.Count == 0)
            //{
            //    var listPhotoName = imageNames.Split("||");
            //    _photoService.DeletePhoto(post.PostId);
            //    foreach (string photoName in listPhotoName)
            //    {
            //        Photo photo = new Photo();
            //        photo.PhotoUrl = photoName;
            //        photo.CreatedAt = DateTime.Now;
            //        photo.PostId = post.PostId;
            //        _photoService.AddPhoto(photo);
            //    }
            //}
            //var p = _postService.GetPostById(post.PostId);
            //if (!p.Caption.Equals(post.Caption))
            //{
            //    _postService.UpdatePost(post);
            //}



            return RedirectToPage("/Profile");
        }
    }
}
