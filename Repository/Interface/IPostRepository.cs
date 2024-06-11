using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface IPostRepository
{
    void CreatePost(Post post);
    List<int> GetAllFollower(int currentUser);
    List<Post> GetAllPost(List<int> listUser);
    List<int> GetAllFriendId(int currentUser);
}