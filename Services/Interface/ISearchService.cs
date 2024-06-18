using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface ISearchService
{
    List<Post> SearchPost(string searchTerm);

    List<User> SearchUser(string searchTerm);
}