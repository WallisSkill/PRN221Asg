using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages;

[Authorize]
[RequiredArgsConstructor]
public partial class Profile : PageModel
{
    private readonly IProfileService _profileService;
    
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    public IActionResult OnGet()
    {
        if (Id == 0) return RedirectToPage("/Index");
        ViewData["User"] = _profileService.GetUserInfo(Id);
        ViewData["photos"] = _profileService.GetUserPhoto(Id);
        ViewData["likes"] = _profileService.GetCountNumberLikes(Id);
        ViewData["comments"] = _profileService.GetCountNumberComments(Id);
        return Page();
    }
}