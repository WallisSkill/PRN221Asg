using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.Extensions.Hosting;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;
[Service]
public partial class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly ISignUpRepository _signUpRepository;
    private readonly int _currentUser;

    public PostService(IPostRepository postRepository, ISignUpRepository signUpRepository, IUserResolverService userResolverService)
    {
        _postRepository = postRepository;
        _currentUser = userResolverService.GetUser();
        _signUpRepository = signUpRepository;
    }
    public void CreatePost(Post post)
    {
        _postRepository.CreatePost(post);
    }

    private List<int> GetAllFriendAndFollowerId()
    {
        var listFriendId = _postRepository.GetAllFriendId(_currentUser);
        var listFollowerId = _postRepository.GetAllFollower(_currentUser);
        listFollowerId.AddRange(listFriendId);
        return listFollowerId.Distinct().ToList();
    }

    private List<int> GetAllUser()
    {
        var listFriendId = _postRepository.GetAllUserId();
        
        return listFriendId.Distinct().ToList();
    }

    public List<PostData> GetAllPostOfFriendAndFollower(int Id = 0)
    {
        var listUsers = new List<int>();
        if (Id > 0)
        {
            listUsers.Add(Id);
        }
        else
        {
            listUsers = GetAllFriendAndFollowerId();
            if(Id == -1)
            {
                listUsers = GetAllUser();
            }
        }
        var listPost = _postRepository.GetAllPost(listUsers.ToList());

        var listPostId = listPost.Select(x => x.Id).Distinct().ToList();
        var listComment = _postRepository.GetAllComments(listPostId);
        var listCommentId = listComment.Select(x => x.CommentId).Distinct().ToList();

        var listCmtLikes = _postRepository.GetAllCommentsLike(listCommentId);
        var listPostLikes = _postRepository.GetAllPostsLike(listPostId);

        listComment.ForEach(item =>
        {
            item.CmtLikes = listCmtLikes.Where(x => x.ConnectId == item.CommentId).ToList();
        });

        listPost.ForEach(item =>
        {
            var listCmtOfPost = listComment.Where(x => x.PostId == item.Id).ToList();
            if (listCmtOfPost.Count > 0)
            {
                item.ListComments = HandleCommentDatas(listCmtOfPost);
                item.countComment = listCmtOfPost.Count;
                item.ListCommentsTotal = listCmtOfPost;
            }
            item.ListLike = listPostLikes.Where(x => x.ConnectId == item.Id).ToList();
        });

        return listPost;
    }

    public Comment InsertComment(Comment comment)
    {
        comment.UserId = _currentUser;
        comment.CreatedAt = DateTime.Now;
        return _postRepository.InsertComment(comment);
    }

    private List<CommentData> HandleCommentDatas(List<CommentData> comments)
    {
        var tree = comments.GroupBy(x => x.CmtParent).ToDictionary(x => x.Key, x => x.OrderBy(c => c.Time).ToList());
        var result = new List<CommentData>();

        void DFS(CommentData comment)
        {
            if (tree.ContainsKey(comment.CommentId))
            {
                tree[comment.CommentId].ForEach(item =>
                {
                    DFS(item);
                });
                comment.ListComment = tree[comment.CommentId];
                comment.CountChildCmt = tree[comment.CommentId].Sum(x => x.CountChildCmt) + tree[comment.CommentId].Count;
            }
            if (comment.CmtParent == 0)
            {
                result.Add(comment);
            }
        }

        tree[0].ForEach(item => DFS(item));

        return result;
    }

    public List<LikeData> GetLikeDataOfPostAfterLike(int postId, int emotionId, bool deleteStatus)
    {
        HandleLikePost(postId, emotionId, deleteStatus);
        var likeDataOfPost = _postRepository.GetAllPostsLike(new List<int> { postId });
        return likeDataOfPost;
    }

    public List<LikeData> GetLikeDataOfCmt(int cmtId, int emotionId)
    {
        HandleLikeCmt(cmtId, emotionId);
        var likeDataOfCmt = _postRepository.GetAllCommentsLike(new List<int> { cmtId });
        return likeDataOfCmt;
    }

    private void HandleLikeCmt(int cmtId, int emotionId)
    {
        var cmtLike = _postRepository.GetCmtLike(cmtId, _currentUser);
        if (cmtLike == null)
        {
            if (emotionId == 0)
            {
                _postRepository.InsertCmtLike(cmtId, _currentUser, 1);
            }
            else
            {
                _postRepository.InsertCmtLike(cmtId, _currentUser, emotionId);
            }
        }
        else
        {
            if (emotionId == 0)
            {
                _postRepository.DeleteCmtLike(cmtId, _currentUser);
            }
            else
            {
                _postRepository.UpdateCmtLike(cmtId, _currentUser, emotionId);
            }
        }
    }

    private void HandleLikePost(int postId, int emotionId, bool deleteStatus)
    {
        var postLike = _postRepository.GetPostLike(postId, _currentUser);
        if (postLike != null)
        {
            if (deleteStatus)
            {
                _postRepository.DeletePostLike(postId, _currentUser);
            }
            else
            {
                _postRepository.UpdatePostLike(postId, _currentUser, emotionId);
            }
        }
        else
        {
            _postRepository.InsertPostLike(postId, _currentUser, emotionId);
        }
    }
    //Saved
    public List<int> GetAllPostIdsaved()
    {
        var listPostId = _postRepository.GetAllPostIdsaved(_currentUser);
        return listPostId.Distinct().ToList();
    }
    public List<PostData> GetAllPostCurrentUserSaved()
    {
        var listPost = _postRepository.GetAllPostSaved(GetAllPostIdsaved(), _currentUser);

        var listPostId = listPost.Select(x => x.Id).Distinct().ToList();
        var listComment = _postRepository.GetAllComments(listPostId);
        var listCommentId = listComment.Select(x => x.CommentId).Distinct().ToList();

        var listCmtLikes = _postRepository.GetAllCommentsLike(listCommentId);
        var listPostLikes = _postRepository.GetAllPostsLike(listPostId);

        listComment.ForEach(item =>
        {
            item.CmtLikes = listCmtLikes.Where(x => x.ConnectId == item.CommentId).ToList();
        });

        listPost.ForEach(item =>
        {
            var listCmtOfPost = listComment.Where(x => x.PostId == item.Id).ToList();
            if (listCmtOfPost.Count > 0)
            {
                item.ListComments = HandleCommentDatas(listCmtOfPost);
                item.countComment = listCmtOfPost.Count;
                item.ListCommentsTotal = listCmtOfPost;
            }
            item.ListLike = listPostLikes.Where(x => x.ConnectId == item.Id).ToList();
        });

        return listPost;
    }

    public void SavePost(int postId)
    {
        _postRepository.SavePost(postId, _currentUser);
    }

    public void RemovePost(int postId)
    {
        _postRepository.RemovePost(postId, _currentUser);
    }

    public void DeletePost(int postId)
    {
        _postRepository.DeletePost(postId);
    }

    public void UpdatePost(Post post)
    {
        _postRepository.UpdatePost(post);
    }

    public Post GetPostById(int postId)
    {
       return _postRepository.GetPostById(postId);
    }

    public void RemoveComment(int cmdId)
    {
        _postRepository.RemoveComment(cmdId);
    }

    public async Task<IList<LikeData>> GetAllPostLikeNoti()
    {
        DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
        var post = GetAllPostOfFriendAndFollower(_currentUser);
        var postLikes = _postRepository.GetAllPostsLike(
            post.Select(x=> x.Id)
            .ToList()
            ).Where(x => x.createDate >= fiveMinutesAgo && x.User.UserId != _currentUser)
        .ToList();
        return await Task.FromResult(postLikes);
    }

    public async Task<IList<CommentData>> GetAllCommentNoti()
    {
        DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
        var post = GetAllPostOfFriendAndFollower(_currentUser);
        var comments = _postRepository.GetAllComments(
            post.Select(x => x.Id)
            .ToList()
            ).Where(x => x.Time >= fiveMinutesAgo && x.CmtParent == 0 && x.User.UserId != _currentUser)
            .ToList();
        return await Task.FromResult(comments);
    }

    public async Task<IList<CommentData>> GetAllCommentReplyNoti()
    {
        DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);

        List<int> allUser = _signUpRepository.GetUsers().Select(x => x.UserId).ToList();
        var post = _postRepository.GetAllPost(allUser);

        var commentOfUsers = _postRepository.GetAllCommentsOfUsers(_currentUser);

        var postIds = post.Select(p => p.Id).ToList();
        var allComments = _postRepository.GetAllComments(postIds);

        var commentReplies = allComments
            .Where(x => x.Time >= fiveMinutesAgo &&
                        x.CmtParent > 0 &&
                        commentOfUsers
                            .Select(y => y.CommentId)
                            .Contains(x.CmtParent) &&
                        x.User.UserId != _currentUser)
            .ToList();

        return await Task.FromResult(commentReplies);
    }






    public async Task<Post> GetPostAsyncById(int postId)
    {
        return await _postRepository.GetPostAsyncById(postId);
    }

    public async Task<IList<LikeData>> GetAllCommentLikesNoti()
    {
        DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
        var comments= _postRepository.GetAllCommentsOfUsers(_currentUser).ToList();
        var likedComments = _postRepository.GetAllCommentsLike(
            comments.Select(
                x => x.CommentId)
            .ToList()).Where(x => x.createDate > fiveMinutesAgo).ToList();
        return await Task.FromResult(likedComments);
    }

    public async Task<IList<CommentData>> GetAllComments()
    {
        var comments = _postRepository.GetAllCommentsOfUsers(_currentUser).ToList();
        return await Task.FromResult(comments); ;
    }

    public List<PostData> GetAllPost()
    {
        List<int> allUser = _signUpRepository.GetUsers().Select(x => x.UserId).ToList();
        return _postRepository.GetAllPost(allUser);
    }
}