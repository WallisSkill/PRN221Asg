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
        public string error;

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            var userLogin = _loginService.ExistUser(user.Username, user.Password);

            if (userLogin != null)
            {
                List<Claim> listClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userLogin.Fullname),
                new Claim(ClaimTypes.Name, userLogin.Username),
                new Claim(ClaimTypes.Email, userLogin.Email),
                new Claim("profile_picture", userLogin.ProfilePhotoUrl ?? "./assets/images/user/null.png")
            };
                ClaimsIdentity ci = new ClaimsIdentity(listClaim, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal cp = new ClaimsPrincipal(ci);
                HttpContext.SignInAsync(cp);
                return RedirectToPage("/Index");
            }
            error = "Username or password is incorrect!";
            return Page();
        }
        
        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Login");
        }
    }
}
