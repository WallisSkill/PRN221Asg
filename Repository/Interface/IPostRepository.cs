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
    Comment InsertComment(Comment comment);
    PostLike GetPostLike(int postId, int currentUser);
    void UpdatePostLike(int postId, int currentUser, int emotionId);
    void InsertPostLike(int postId, int currentUser, int emotionId);
    void DeletePostLike(int postId, int currentUser);
}