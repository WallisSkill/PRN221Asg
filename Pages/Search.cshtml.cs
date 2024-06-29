using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services;
using PRN221_Assignment.Services.Interface;

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
            ViewData["searchPost"] = _searchService.SearchPost(searchTerm.Trim());
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

        public void OnGet(string searchTerm, string SeeAll)
        {
            this.searchTerm = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                ViewData["searchPost"] = _searchService.SearchPost(searchTerm.Trim());
                ViewData["userFriends"] = _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
                ViewData["searchUser"] = _searchService.SearchUser(searchTerm.Trim());
                ViewData["searchTerm"] = searchTerm;
                ViewData["listPost"] = _postService.GetAllPostOfFriendAndFollower();
                ViewData["listSaved"] = _postService.GetAllPostIdsaved();
            }
        }

        public IActionResult OnGetGetSuggestions(string query)
        {
            var listFriend = _profileService.GetAllFriendOfUser(_userResolver.GetUser());
            var listUser = _searchService.SearchUser(query.Trim());
            var suggestions = listFriend
                .Where(u => u.User.Fullname.Contains(query, System.StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .Select(u => new { u.User.UserId, u.User.Fullname, u.User.profilePhotoUrl, IsFriend = true })
                .ToList();
            if (suggestions.Count == 0)
            {
                suggestions = listUser
                    .Where(u => u.Fullname.Contains(query, System.StringComparison.OrdinalIgnoreCase))
                    .Take(5)
                    .Select(u => new { u.UserId, u.Fullname, u.profilePhotoUrl, IsFriend = false })
                    .ToList();
            }
            return new JsonResult(suggestions);
        }
    }
}
