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
    public List<Post> GetAllPost(List<int> listUser)
    {
        var query = _context.Set<Post>().Where(x => listUser.Contains(x.UserId));
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
}