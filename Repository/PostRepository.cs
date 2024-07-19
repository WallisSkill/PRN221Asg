using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
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
                        createDate = T1.CreatedAt
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
                        createDate = T1.CreatedAt
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

    public List<PostData> GetAllPostSaved(List<int> listPosId, int currentUserId)
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
    public void DeletePost(int postId)
    {
        var post = _context.Posts.FirstOrDefault(x => x.PostId == postId);
        var photo = _context.Photos.Where(x => x.PostId == postId).ToList();
        var comment = _context.Comments.Where(x => x.PostId == postId).ToList();
        var bookmark = _context.Bookmarks.Where(x => x.PostId == postId).ToList();
        var post_like = _context.PostLikes.Where(x => x.PostId == postId).ToList();
        foreach (var item in comment)
        {
            var comment_like = _context.CommentLikes.Where(x => x.CommentId == item.CommentId);
            _context.CommentLikes.RemoveRange(comment_like);
        }
        _context.PostLikes.RemoveRange(post_like);
        _context.Bookmarks.RemoveRange(bookmark);
        _context.Comments.RemoveRange(comment);
        _context.Photos.RemoveRange(photo);
        _context.Posts.Remove(post);
        _context.SaveChanges();
    }

    public void UpdatePost(Post post)
    {
        var p = _context.Posts.Find(post.PostId);
        if (p != null)
        {
            p.Caption = post.Caption;
            _context.Posts.Update(p);
            _context.SaveChanges();
        }

    }

    public CommentLike GetCmtLike(int cmtId, int currentUser)
    {
        var query = _context.Set<CommentLike>().FirstOrDefault(x => x.CommentId == cmtId && currentUser == x.UserId);
        return query;
    }

    public void UpdateCmtLike(int cmtId, int currentUser, int emotionId)
    {
        var postLike = _context.Set<CommentLike>().FirstOrDefault(x => x.CommentId == cmtId && x.UserId == currentUser);
        postLike.EmotionId = emotionId;
        _context.SaveChanges();
    }

    public void InsertCmtLike(int cmtId, int currentUser, int emotionId)
    {
        _context.Set<CommentLike>().Add(new CommentLike()
        {
            CommentId = cmtId,
            UserId = currentUser,
            EmotionId = emotionId,
            CreatedAt = DateTime.Now,
        });
        _context.SaveChanges();
    }

    public void DeleteCmtLike(int cmtId, int currentUser)
    {
        var cmtLike = _context.Set<CommentLike>().FirstOrDefault(x => x.CommentId == cmtId && x.UserId == currentUser);
        _context.Set<CommentLike>().Remove(cmtLike);
        _context.SaveChanges();
    }

    public Post GetPostById(int postId)
    {
        return _context.Posts.FirstOrDefault(x => x.PostId == postId);
    }

    public List<int> GetAllUserId()
    {
        return _context.Users.Select(x => x.UserId).ToList();
    }

    public void RemoveComment(int cmdId)
    {
        var comments = _context.Comments.Where(x => x.ParentId == cmdId).ToList();
        foreach (var item in comments)
        {
            RemoveComment(item.CommentId);
        }
        var cmtLike = _context.CommentLikes.Where(x => x.CommentId == cmdId).ToList();
        _context.CommentLikes.RemoveRange(cmtLike);
        var cmt = _context.Comments.FirstOrDefault(x => x.CommentId == cmdId);
        _context.Comments.Remove(cmt);
        _context.SaveChanges();
    }

    public Comment? getCommentById(int v)
    {
        return _context.Set<Comment>().Where(x => x.CommentId == v).FirstOrDefault();
    }

    public List<CommentData> GetAllCommentsOfUsers(int currentUser)
    {
        var query = from T1 in _context.Set<Comment>()
                    where T1.UserId == currentUser
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

    public async Task<Post> GetPostAsyncById(int postId)
    {
        return await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
    }

    public async Task<IList<PostData>> GetAllPostAsync(List<int> listFriendId)
    {
        DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
        var query = from T1 in _context.Set<Post>()
                    where listFriendId.Contains(T1.UserId)
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
        return await query.Where(x => x.Time >= fiveMinutesAgo).ToListAsync();
    }

    public CommentLike GetCommentLike(int v1, int v2)
    {
        var query = _context.Set<CommentLike>().FirstOrDefault(x => x.CommentId == v1 && v2 == x.UserId);
        return query;
    }
}