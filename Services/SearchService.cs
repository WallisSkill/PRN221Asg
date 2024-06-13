using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;
[RequiredArgsConstructor]
[Service]
public partial class SearchService : ISearchService
{
    private readonly ISearchRepository _searchRepository;

    public List<Post> SearchPost(string searchTerm)
    {
        return _searchRepository.SearchPost(searchTerm);
    }

    public List<User> SearchUser(string searchTerm)
    {
        return _searchRepository.SearchUser(searchTerm);
    }
}