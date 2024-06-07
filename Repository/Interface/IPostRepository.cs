using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface IPostRepository
{
    void CreatePost(Post post);

    List<Post> GetPostsByUserId(int userId);
}