using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services;
using PRN221_Assignment.Services.Interface;
using System.Linq;

namespace PRN221_Assignment.Pages
{
    [Authorize]
    [RequiredArgsConstructor]
    public partial class SearchModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string searchTerm { get; set; }

        private readonly ISearchService _searchService;
        private readonly IUserResolverService _userResolver;
        private readonly IProfileService _profileService;
        private readonly IPostService _postService;

        public IActionResult OnPost()
        {
            if (searchTerm == null) return RedirectToPage("/Index");
            var a = _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
            ViewData["userFriends"] = a;
            ViewData["searchUser"] = _searchService.SearchUser(searchTerm.Trim());
            ViewData["searchTerm"] = searchTerm;
            ViewData["listPost"] = _postService.GetAllPostOfFriendAndFollower();
            ViewData["listSaved"] = _postService.GetAllPostIdsaved();
            return RedirectToPage(new
            {
                searchTerm = searchTerm,
            });
        }

        public void OnGet(string searchTerm)
        {
            this.searchTerm = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                ViewData["userFriends"] = _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
                ViewData["searchUser"] = _searchService.SearchUser(searchTerm.Trim());
                ViewData["searchTerm"] = searchTerm;
                ViewData["listPost"] = _postService.GetAllPostOfFriendAndFollower();
                ViewData["listSaved"] = _postService.GetAllPostIdsaved();

            }
        }

        public IActionResult OnGetGetSuggestions(string query)
        {
            var currentUser = _userResolver.GetUser();
            var listFriend = _profileService.GetAllFriendOfUser(currentUser);
            var listUser = _searchService.SearchUser(query.Trim());

            var friendIds = new List<int>(listFriend.Select(f => f.User.UserId));

            var suggestions = listUser
                .Where(u => u.Fullname.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .Select(u => new
                {
                    u.UserId,
                    u.Fullname,
                    IsFriend = friendIds.Contains(u.UserId)
                })
                .ToList();

            return new JsonResult(suggestions);
        }
    }
}
