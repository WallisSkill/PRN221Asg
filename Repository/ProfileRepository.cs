using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository;

[Service]
[RequiredArgsConstructor]
public partial class ProfileRepository: IProfileRepository
{
    private readonly social_mediaContext _context;
    
    public User? GetUserInfo(int userid)
    {
        return _context.Set<User>().FirstOrDefault(x => x.UserId == userid);
    }

    public List<Photo> GetUserPhoto(int userid)
    {
        var result = (from photos in _context.Set<Photo>()
            join post in _context.Set<Post>() on photos.PostId equals post.PostId
            where post.UserId == userid
            orderby photos.CreatedAt
            select photos).ToList();
        return result;
    }
}