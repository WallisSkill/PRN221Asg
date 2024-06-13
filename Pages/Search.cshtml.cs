using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages
{
    [RequiredArgsConstructor]
    public partial class SearchModel : PageModel
    {
        [BindProperty]
        public string searchTerm { get; set; }
        private readonly ISearchService _searchService;
       
        public void OnPost()
        {
            ViewData["searchPost"] = _searchService.SearchPost(searchTerm);
            ViewData["searchUser"] = _searchService.SearchUser(searchTerm);
        }

    }
}
