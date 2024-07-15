using Lombok.NET;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.ExtenModel;
using PRN221_Assignment.Services.Interface;
using System.Security.Claims;

namespace PRN221_Assignment.Pages
{
    [RequiredArgsConstructor]
    public partial class LoginAdminModel : PageModel
    {
        [BindProperty]
        public ExtenModel.Admin? User { get; set; } = default!;
        private readonly ILoginService _loginService;
        private readonly ILogger<LoginAdminModel> _logger;
        public string error;

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            var userLogin = _loginService.ExistUserAdmin(User.Username, User.Key);

            if (userLogin != null)
            {
                List<Claim> listClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userLogin.Username),
                new Claim(ClaimTypes.NameIdentifier, userLogin.Id),
                new Claim(ClaimTypes.Role, "Admin"),
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
