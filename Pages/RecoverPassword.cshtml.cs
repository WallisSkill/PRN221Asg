using Lombok.NET;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Constant;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages
{
    [RequiredArgsConstructor]
    public partial class RecoverPassword : PageModel
    {
        private readonly IMailService _mailService;
        private readonly IRecoverPasswordService _recoverPasswordService;

        [BindProperty]
        public string mail { get; set; }
        public string error;
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var user = _recoverPasswordService.GetUserByMail(mail);
            if (user != null)
            {
                _mailService.SendMail(mail, "Recover Password", $"Hello {user.Email}<br>" + ConstantMail.RecoverPasswordContent.Replace("ReplacePass", user.Password));
                return RedirectToPage("/Login");
            }
            error = "Email is not exist!";
            return Page();
        }
    }
}
