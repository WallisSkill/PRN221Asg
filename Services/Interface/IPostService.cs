using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface IPostService
{
    void CreatePost(Post post);

    List<Post> GetPostsByUserId(int userId);
}