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
    public partial class EditProfileModel : PageModel
    {
        private readonly IProfileService _profileService;
        private readonly IUserResolverService userResolverService;

        [BindProperty]
        public User user { get; set; }
        public IActionResult OnGet()
        {
            user = _profileService.GetUserInfo(userResolverService.GetUser());
            return Page();
        }

        public IActionResult OnPost(User user)
        {
            _profileService.EditProfile(user);
            return Page();
        }
    }
}
