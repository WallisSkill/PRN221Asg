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
    private readonly IUserResolverService _userResolver;
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    public IActionResult OnGet()
    {
        if (Id == 0) return RedirectToPage("/Index");
        ViewData["User"] = _profileService.GetUserInfo(Id);
        ViewData["photos"] = _profileService.GetUserPhoto(Id);
        ViewData["likes"] = _profileService.GetCountNumberLikes(Id);
        ViewData["comments"] = _profileService.GetCountNumberComments(Id);
        var b = _profileService.GetAllFriendOfUser(Id);
        ViewData["friends"] = b;
        var a =  _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
        ViewData["userFriends"] = a;
        return Page();
    }
}