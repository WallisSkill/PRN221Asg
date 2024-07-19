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
using System.Text.RegularExpressions;

namespace PRN221_Assignment.Pages.Admin;

[Authorize(Roles = "Admin")]
[RequiredArgsConstructor]
public partial class ProfileAdmin : PageModel
{
    private readonly IProfileService _profileService;
    private readonly IUserResolverService _userResolver;
    private readonly IPostService _postService;
    private readonly IAdminService _adminService;
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
        var posts = _postService.GetAllPostOfFriendAndFollower(-1);
        var badwords = new HashSet<string>(_adminService.GetAllBadWords(), StringComparer.OrdinalIgnoreCase);

        var badWordPosts = posts.Where(post =>
            !string.IsNullOrEmpty(post.Caption) &&
            badwords.Any(badword => Regex.IsMatch(post.Caption, $@"\b{Regex.Escape(badword)}\b", RegexOptions.IgnoreCase)))
            .ToList();

        ViewData["listPost"] = badWordPosts;

        var b = _adminService.GetUsers();
        ViewData["friends"] = b;
        var a = _adminService.GetBlockedAccount();
        ViewData["userFriends"] = a;
        return Page();
    }

    public IActionResult OnPostUpdateLock(int userId)
    {
        var a = _adminService.GetBlockedAccount();
        if (a.Any(x => x.User1Id == userId))
        {
            _adminService.UpdateLock(userId, false);
        }
        else
        {
            _adminService.UpdateLock(userId, true);
        }
        
        return new JsonResult("ok");
    }

    public IActionResult OnDeleteFollow(int UserId)
    {
        _profileService.UnFollow(_userResolver.GetUser(), UserId);
        return new JsonResult("ok");
    }
}