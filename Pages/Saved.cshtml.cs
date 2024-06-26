using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages
{
    [Authorize]
    [RequiredArgsConstructor]
    public partial class SavedModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHomePageService _homePageService;
        private readonly IMessageService _messageService;
        private readonly IPostService _postService;
        public IActionResult OnGet()
        {
            ViewData["listPost"] = _postService.GetAllPostCurrentUserSaved();
            ViewData["listSaved"] = _postService.GetAllPostIdsaved();
            return Page();
        }

        public void OnPost(int PostId, bool type)
        {
            if (type)
            {
                _postService.SavePost(PostId);
            }
            else
            {
                _postService.RemovePost(PostId);
            }
        }
    }
}
