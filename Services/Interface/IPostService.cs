using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface IPostService
{
    void CreatePost(Post post);
    List<PostData> GetAllPostOfFriendAndFollower(bool onnlyCurrentUser = false);
}