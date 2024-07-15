using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface IPostService
{
    void CreatePost(Post post);
    List<PostData> GetAllPostOfFriendAndFollower(int Id =0);
    Comment InsertComment(Comment comment);
    List<LikeData> GetLikeDataOfPostAfterLike(int postId, int emotionId, bool deleteStatus);
    List<int> GetAllPostIdsaved();
    List<PostData> GetAllPostCurrentUserSaved();
    void SavePost(int postId);
    void RemovePost(int postId);
    void DeletePost(int postId);
    void UpdatePost(Post post);
    List<LikeData> GetLikeDataOfCmt(int cmtId, int emotionId);
}