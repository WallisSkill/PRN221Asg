using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface ISearchRepository
{
    List<Post> SearchPost(string searchTerm);

    List<User> SearchUser(string searchTerm);
}