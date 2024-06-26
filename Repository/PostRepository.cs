using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.Extensions.Hosting;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

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
                    join T3 in _context.Set<Photo>()
                    on T1.PostId equals T3.PostId into photos
                    from T3 in photos.DefaultIfEmpty() // Left join
                    group new { T1, T3 } by new { T1.PostId, T2.UserId, T2.Fullname, T2.ProfilePhotoUrl, T2.Dob, T1.Caption } into g
                    orderby g.Max(x => x.T1.CreatedAt) descending
                    select new PostData()
                    {
                        Id = g.Key.PostId,
                        User = new User()
                        {
                            UserId = g.Key.UserId,
                            Fullname = g.Key.Fullname,
                            ProfilePhotoUrl = g.Key.ProfilePhotoUrl,
                            Dob = g.Key.Dob,
                        },
                        Caption = g.Key.Caption,
                        PhotoURL = g.Select(x => x.T3 != null ? x.T3.PhotoUrl : null).ToList(),
                        Time = (DateTime)g.Max(x => x.T1.CreatedAt),
                        ListComments = new List<CommentData>(),
                        ListLike = new List<LikeData>(),
                    };
        return query.ToList();
    }

    public List<int> GetAllFriendId(int currentUser)
    {
        var query = from T1 in _context.Set<Friend>()
                    where T1.User1Id == currentUser || T1.User2Id == currentUser
                    select T1.User1Id == currentUser ? T1.User2Id : T1.User1Id;
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

    public Comment InsertComment(Comment comment)
    {
        _context.Set<Comment>().Add(comment);
        _context.SaveChanges();
        return comment;
    }

    public PostLike GetPostLike(int postId, int currentUser)
    {
        var query = _context.Set<PostLike>().FirstOrDefault(x => x.PostId == postId && currentUser == x.UserId);
        return query; 
    }

    public void UpdatePostLike(int postId, int currentUser, int emotionId)
    {
        var postLike = _context.Set<PostLike>().FirstOrDefault(x => x.PostId == postId && x.UserId == currentUser);
        postLike.EmotionId = emotionId;
        _context.SaveChanges();
    }

    public void InsertPostLike(int postId, int currentUser, int emotionId)
    {
        _context.Set<PostLike>().Add(new PostLike()
        {
            PostId = postId,
            UserId = currentUser,
            EmotionId = emotionId,
            CreatedAt = DateTime.Now,
        });
        _context.SaveChanges();
    }

    public void DeletePostLike(int postId, int currentUser)
    {
        var postLike = _context.Set<PostLike>().FirstOrDefault(x => x.PostId == postId && x.UserId == currentUser);
        _context.Set<PostLike>().Remove(postLike);
        _context.SaveChanges();
    }

    public List<PostData> GetAllPostSaved(List<int> listPosId,int currentUserId)
    {
        var query = from T1 in _context.Set<Post>()
                    where listPosId.Contains(T1.PostId)
                    from T2 in _context.Set<User>()
                    where T2.UserId == T1.UserId
                    join T3 in _context.Set<Photo>()
                    on T1.PostId equals T3.PostId into photos
                    from T3 in photos.DefaultIfEmpty()
                    join T4 in _context.Set<Bookmark>()
                    on T1.PostId equals T4.PostId into bookmark
                    from T4 in bookmark.DefaultIfEmpty()
                    where T4.UserId == currentUserId
                    group new { T1, T3, T4 } by new { T1.PostId, T2.UserId, T2.Fullname, T2.ProfilePhotoUrl, T2.Dob, T1.Caption, T4.CreatedAt } into g
                    select new
                    {
                        g.Key.PostId,
                        g.Key.UserId,
                        g.Key.Fullname,
                        g.Key.ProfilePhotoUrl,
                        g.Key.Dob,
                        g.Key.Caption,
                        Photos = g.Select(x => x.T3 != null ? x.T3.PhotoUrl : null).ToList(),
                        CreatedAt = g.Key.CreatedAt,
                        CreatedAtPost = g.Max(x => x.T1.CreatedAt)
                    };

        var orderedQuery = query.OrderByDescending(x => x.CreatedAt).ToList();

        var result = orderedQuery.Select(g => new PostData()
        {
            Id = g.PostId,
            User = new User()
            {
                UserId = g.UserId,
                Fullname = g.Fullname,
                ProfilePhotoUrl = g.ProfilePhotoUrl,
                Dob = g.Dob,
            },
            Caption = g.Caption,
            PhotoURL = g.Photos,
            Time = (DateTime)g.CreatedAtPost,
            ListComments = new List<CommentData>(),
            ListLike = new List<LikeData>(),
        }).ToList();

        return result;
    }

    public List<int> GetAllPostIdsaved(int currentUser)
    {
        var query = _context.Set<Bookmark>().Where(x => x.UserId == currentUser).ToList();
        return query.Select(x => x.PostId).ToList();
    }

    public void SavePost(int postId, int currentUserId)
    {
        Bookmark post = new Bookmark();
        post.UserId = currentUserId;
        post.CreatedAt = DateTime.Now;
        post.PostId = postId;
        _context.Add(post);
        _context.SaveChanges();
    }

    public void RemovePost(int postId, int currentUserId)
    {
        Bookmark? post = _context.Set<Bookmark>().Where(x => x.PostId == postId && x.UserId == currentUserId).FirstOrDefault();
        if (post != null)
        {
            _context.Remove(post);
            _context.SaveChanges();
        }
       
    }
}