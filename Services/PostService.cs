using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;
[RequiredArgsConstructor]
[Service]
public partial class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    public void CreatePost(Post post)
    {
        _postRepository.CreatePost(post);
    }
}