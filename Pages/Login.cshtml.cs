using Lombok.NET;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;
using System.Security.Claims;

namespace PRN221_Assignment.Pages
{
    [RequiredArgsConstructor]
    public partial class LoginModel : PageModel
    {
        [BindProperty]
        public User user { get; set; } = default!;
        private readonly ILoginService _loginService;
        private readonly ILogger<LoginModel> _logger;
        private readonly IAdminService _adminService;
        private readonly IUserResolverService _userResolver;
        public string error;

        [BindProperty]
        public string ReturnUrl { get; set; }
        public void OnGet(string returnUrl = "Index")
        {
            ReturnUrl = returnUrl;
            HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public IActionResult OnPost(string returnUrl)
        {
            var userLogin = _loginService.ExistUser(user.Username, user.Password);
            var blocked = _adminService.GetBlockedAccount();
            bool isBlocked = false;
            error = "Username or password is incorrect!";
            
            if (userLogin != null && !isBlocked)
            {
                List<Claim> listClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, userLogin.Fullname),
                new Claim(ClaimTypes.Name, userLogin.Username),
                new Claim(ClaimTypes.NameIdentifier, userLogin.UserId.ToString()),
                new Claim(ClaimTypes.Email, userLogin.Email),
                new Claim("UserId", userLogin.UserId.ToString()),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("profile_picture", userLogin.ProfilePhotoUrl ?? "./assets/images/user/null.png")
            };
                ClaimsIdentity ci = new ClaimsIdentity(listClaim, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal cp = new ClaimsPrincipal(ci);
                HttpContext.SignInAsync(cp);
                if (blocked.Any(x => x.User1Id == userLogin.UserId))
                {
                    error = "Your account has been disabled";
                    isBlocked = true;
                    return Page();
                }
                return RedirectToPage(returnUrl);
            }
            
            return Page();
        }
        
        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Login");
        }

        public async Task<IActionResult> OnPostCheckingLock()
        {
            var blocked = _adminService.GetBlockedAccount();
            if (blocked.Any(x => x.User1Id == _userResolver.GetUser()))
            {
                error = "Your account has been disabled";
                await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
                return new JsonResult(new { success = true, message = error });
            }
            return new JsonResult(new { success = false });

        }
    }
}
