using DependencyInjectionAutomatic.Service;
using Lombok.NET;
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

    public List<Post> GetAllPostOfFriendAndFollower()
    {
        var listFriendId = _postRepository.GetAllFriendId(_currentUser);
        var listFollowerId = _postRepository.GetAllFollower(_currentUser);
        listFollowerId.AddRange(listFriendId);
        var listPost = _postRepository.GetAllPost(listFollowerId.Distinct().ToList());
        return listPost;
    }
}