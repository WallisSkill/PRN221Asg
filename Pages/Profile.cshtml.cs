using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services;
using PRN221_Assignment.Services.Interface;
using System;
using System.IO;
using System.Text;

namespace PRN221_Assignment.Pages;

[Authorize]
[RequiredArgsConstructor]
public partial class Profile : PageModel
{
    private readonly IProfileService _profileService;
    private readonly IUserResolverService _userResolver;
    private readonly IPostService _postService;
    private int? _id;
    [BindProperty(SupportsGet = true)]
    public int Id
    {
        get => _id ?? _userResolver.GetUser();
        set => _id = value;
    }
    public Post post { get; set; }
    public IActionResult OnGet()
    {
        if (Id == 0) return RedirectToPage("/Index");
        ViewData["User"] = _profileService.GetUserInfo(Id);
        ViewData["photos"] = _profileService.GetUserPhoto(Id);
        ViewData["likes"] = _profileService.GetCountNumberLikes(Id);
        ViewData["comments"] = _profileService.GetCountNumberComments(Id);
        var b = _profileService.GetAllFriendOfUser(Id);
        ViewData["friends"] = b;
        var a = _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
        ViewData["userFriends"] = a;
        ViewData["listPost"] = _postService.GetAllPostOfFriendAndFollower(Id);
        ViewData["listSaved"] = _postService.GetAllPostIdsaved();
        ViewData["isFollow"] = _profileService.CheckIsFollow(_userResolver.GetUser(), Id);
        ViewData["following"] = _profileService.GetCountNumberfollowing(Id);
        ViewData["follower"] = _profileService.GetCountNumberFollower(Id);
        return Page();
    }

    public IActionResult OnPostFollow(int UserId)
    {
        _profileService.Follow(_userResolver.GetUser(), UserId);
        return new JsonResult("ok");
    }

    public IActionResult OnDeleteFollow(int UserId)
    {
        _profileService.UnFollow(_userResolver.GetUser(), UserId);
        return new JsonResult("ok");
    }
}