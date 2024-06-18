using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface IPostRepository
{
    void CreatePost(Post post);
    List<int> GetAllFollower(int currentUser);
    List<PostData> GetAllPost(List<int> listUser);
    List<int> GetAllFriendId(int currentUser);
    List<CommentData> GetAllComments(List<int> listPostId);
    List<LikeData> GetAllCommentsLike(List<int> listCmtId);
    List<LikeData> GetAllPostsLike(List<int> listPostId);
}