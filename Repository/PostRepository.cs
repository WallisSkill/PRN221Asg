using DependencyInjectionAutomatic.Service;
using Lombok.NET;
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

    public List<Post> GetPostsByUserId(int userId)
    {
        return (from p in _context.Set<Post>()
            join f in _context.Set<Friend>() 
            on p.UserId equals f.User2Id
            where f.User1Id == userId
            select p).ToList();
    }
}