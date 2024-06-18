using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository;

[RequiredArgsConstructor]
[Service]
public partial class PostRepository : IPostRepository
{
    private readonly social_mediaContext _context;
    public void CreatePost(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
    }
    public List<PostData> GetAllPost(List<int> listUser)
    {
        var query = from T1 in _context.Set<Post>()
                    where listUser.Contains(T1.UserId)
                    from T2 in _context.Set<User>()
                    where T2.UserId == T1.UserId
                    from T3 in _context.Set<Photo>()
                    where T3.PhotoId == T1.PhotoId
                    select new PostData()
                    {
                        Id = T1.PostId,
                        User = T2,
                        Caption = T1.Caption,
                        PhotoURL = T3.PhotoUrl,
                        Time = (DateTime)T1.CreatedAt,
                        ListComments = new List<CommentData>(),
                        ListLike = new List<LikeData>(),
                    };
        return query.ToList();
    }

    public List<int> GetAllFriendId(int currentUser)
    {
        var query = from T1 in _context.Set<Friend>()
                    where T1.User1Id == currentUser || T1.User2Id == currentUser
                    select T1.User1Id == currentUser ? T1.User2Id : T1.User2Id;
        return query.ToList();
    }

    public List<int> GetAllFollower(int currentUser)
    {
        var query = from T1 in _context.Set<Follow>()
                    where T1.FolloweeId == currentUser
                    select T1.FollowerId;
        return query.ToList();
    }

    public List<CommentData> GetAllComments(List<int> listPostId)
    {
        var query = from T1 in _context.Set<Comment>()
                    where listPostId.Contains(T1.PostId)
                    from T2 in _context.Set<User>()
                    where T1.UserId == T2.UserId
                    select new CommentData()
                    {
                        PostId = T1.PostId,
                        CommentId = T1.CommentId,
                        User = T2,
                        Comment = T1.CommentText,
                        Time = (DateTime)T1.CreatedAt,
                        CmtParent = (int)T1.ParentId,
                        CmtLikes = new List<LikeData>(),
                    };
        return query.ToList();
    }

    public List<LikeData> GetAllCommentsLike(List<int> listCmtId)
    {
        var query = from T1 in _context.Set<CommentLike>()
                    where listCmtId.Contains(T1.CommentId)
                    from T2 in _context.Set<User>()
                    where T1.UserId == T2.UserId
                    from T3 in _context.Set<Emotion>()
                    where T1.EmotionId == T3.EmotionId
                    select new LikeData()
                    {
                        ConnectId = T1.CommentId,
                        User = T2,
                        EmotionURL = T3.EmotionUrl,
                    };
        return query.ToList();
    }

    public List<LikeData> GetAllPostsLike(List<int> listPostId)
    {
        var query = from T1 in _context.Set<PostLike>()
                    where listPostId.Contains(T1.PostId)
                    from T2 in _context.Set<User>()
                    where T1.UserId == T2.UserId
                    from T3 in _context.Set<Emotion>()
                    where T1.EmotionId == T3.EmotionId
                    select new LikeData()
                    {
                        ConnectId = T1.PostId,
                        User = T2,
                        EmotionURL = T3.EmotionUrl,
                    };
        return query.ToList();
    }
}