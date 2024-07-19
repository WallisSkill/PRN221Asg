using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PRN221_Assignment.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Admin")){
                //await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Admin/Login");
            }
            return RedirectToPage("/Login");
        }
    }
}
