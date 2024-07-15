using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface IPostRepository
{
    void CreatePost(Post post);
    List<int> GetAllFollower(int currentUser);
    List<PostData> GetAllPost(List<int> listUser);
    List<PostData> GetAllPostSaved(List<int> listPosId,int currentUserId);
    List<int> GetAllFriendId(int currentUser);
    List<CommentData> GetAllComments(List<int> listPostId);
    List<LikeData> GetAllCommentsLike(List<int> listCmtId);
    List<LikeData> GetAllPostsLike(List<int> listPostId);
    Comment InsertComment(Comment comment);
    PostLike GetPostLike(int postId, int currentUser);
    void UpdatePostLike(int postId, int currentUser, int emotionId);
    void InsertPostLike(int postId, int currentUser, int emotionId);
    void DeletePostLike(int postId, int currentUser);
    List<int> GetAllPostIdsaved(int currentUser);
    void SavePost(int postId, int currentUserid);
    void RemovePost(int postId, int currentUserid);
    void DeletePost(int postId);
    void UpdatePost(Post post);
    CommentLike GetCmtLike(int cmtId, int currentUser);
    void UpdateCmtLike(int cmtId, int currentUser, int emotionId);
    void InsertCmtLike(int cmtId, int currentUser, int emotionId);
    void DeleteCmtLike(int cmtId, int currentUser);
}