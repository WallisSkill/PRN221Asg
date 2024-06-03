using Lombok.NET;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages
{
    [RequiredArgsConstructor]
    public partial class SignUpModel : PageModel
    {
        [BindProperty]
        public User user { get; set; } = default!;
        private readonly ISignUpService _signupService;
        private readonly ILogger<LoginModel> _logger;

        private string error;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (_signupService.CheckEmailExist(user.Email))
            {
                return Page();
            }
            if (_signupService.CheckUsernameExist(user.Email))
            {
                return Page();
            }
            try
            {
                _signupService.InsertUser(user);
                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Page();
            }
        }
    }
}
