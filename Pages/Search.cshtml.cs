using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages
{
    [Authorize]
    [RequiredArgsConstructor]
    public partial class SearchModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string searchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SeeAll { get; set; }

        private readonly ISearchService _searchService;
        private readonly IUserResolverService _userResolver;
        private readonly IProfileService _profileService;

        public IActionResult OnPost()
        {
            if (SeeAll == null) return RedirectToPage("/Index");
            ViewData["searchPost"] = _searchService.SearchPost(searchTerm.Trim());
            var a = _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
            ViewData["userFriends"] = a;
            if (SeeAll == "true")
            {
                ViewData["searchUser"] = _searchService.SearchUser(searchTerm.Trim());
            }
            else
            {
                ViewData["searchUser"] = _searchService.SearchUser(searchTerm.Trim()).Take(3).ToList();
            }
            ViewData["SeeAll"] = SeeAll;
            ViewData["searchTerm"] = searchTerm;
            return RedirectToPage(new
            {
                searchTerm = searchTerm,
                SeeAll = SeeAll
            });
        }
        
        public void OnGet(string searchTerm, string SeeAll)
        {
            this.searchTerm = searchTerm;
            this.SeeAll = SeeAll;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchTermTrimmed = searchTerm.Trim();
                ViewData["searchPost"] = _searchService.SearchPost(searchTermTrimmed);

                var userFriends = _profileService.GetAllFriendRelatetionshipOfUser(_userResolver.GetUser());
                ViewData["userFriends"] = userFriends;

                if (SeeAll == "true")
                {
                    ViewData["searchUser"] = _searchService.SearchUser(searchTermTrimmed);
                }
                else
                {
                    ViewData["searchUser"] = _searchService.SearchUser(searchTermTrimmed).Take(3).ToList();
                }

                ViewData["SeeAll"] = SeeAll;
                ViewData["searchTerm"] = searchTerm;
            }
        }
    }
}
