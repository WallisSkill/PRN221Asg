using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;
[Service]
public partial class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly int _currentUser;

    public PostService(IPostRepository postRepository, IUserResolverService userResolverService)
    {
        _postRepository = postRepository;
        _currentUser = userResolverService.GetUser();
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

    public List<PostData> GetAllPostOfFriendAndFollower(bool onlyCurrentUser = false)
    {
        var listUsers = new List<int>();
        if(onlyCurrentUser)
        {
            listUsers.Add(_currentUser);
        }
        else
        {
            listUsers = GetAllFriendAndFollowerId();
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
}