using Lombok.NET;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;
using System.Security.Claims;


namespace PRN221_Assignment.Pages
{
    [Authorize]
    [RequiredArgsConstructor]
    public partial class EditProfileModel : PageModel
    {
        private readonly IProfileService _profileService;
        private readonly IUserResolverService userResolverService;

        [BindProperty]
        public User? user { get; set; }

        [BindProperty]
        public string error { get; set; }
        public IActionResult OnGet()
        {
            user = _profileService.GetUserInfo(userResolverService.GetUser());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(User user, IFormFile profilePhoto)
        {
        
                if (profilePhoto != null && profilePhoto.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", profilePhoto.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePhoto.CopyToAsync(stream);
                    }

                    user.ProfilePhotoUrl = "./uploads/" + profilePhoto.FileName;
                }

                _profileService.EditProfile(user);
                await UpdateUserClaims(user);
            
            return RedirectToPage("/Profile");
        }

        private async Task UpdateUserClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, user.Fullname),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("profile_picture", user.ProfilePhotoUrl ?? "./assets/images/user/null.png")
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);
        }

        public IActionResult OnPostUpdatePass(string cpass, string npass, string vpass)
        {
            var userid = userResolverService.GetUser();
            var user = _profileService.GetUserInfo(userResolverService.GetUser());
            if(user.Password != cpass)
            {
                error = "Current password is wrong";
                return Page();
            }
            if(npass != vpass)
            {
				error = "Verify password is not matched";
				return Page();
			}

			user.Password = npass;
			
			_profileService.resetPassword(user);
			user = _profileService.GetUserInfo(userResolverService.GetUser());
			return RedirectToPage("/Profile");
		}
    }

}
