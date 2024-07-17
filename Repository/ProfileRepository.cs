using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository;

[Service]
[RequiredArgsConstructor]
public partial class ProfileRepository : IProfileRepository
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

    public int GetCountNumberLikes(int userid)
    {
        var result = (from pl in _context.Set<PostLike>()
            join p in _context.Set<Post>() on pl.PostId equals p.PostId
            where p.UserId == userid
            select pl).Count();
        return result;
    }

    public int GetCountNumberComments(int userid)
    {
        var result = (from c in _context.Set<Comment>()
            join p in _context.Set<Post>() on c.PostId equals p.PostId
            where p.UserId == userid
            select c).Count();
        return result;
    }

    public void EditProfile(User user)
    {
        var userToUpdate = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);

        if (userToUpdate != null)
        {
            userToUpdate.Fullname = user.Fullname;
            userToUpdate.Email = user.Email;
            userToUpdate.Bio = user.Bio;
            userToUpdate.Gender = user.Gender;
            userToUpdate.Dob = user.Dob;
            userToUpdate.profilePhotoUrl = user.ProfilePhotoUrl;
            _context.SaveChanges();
        }
    }

	public bool? CheckIsFollow(int currentUserId ,int id)
	{
		return _context.Set<Follow>().Where(x => x.FollowerId== currentUserId && x.FolloweeId == id).Any();
	}

    public void Follow(int v, int id)
    {
       _context.Set<Follow>().Add(new Follow()
       {
           FollowerId = v,
           FolloweeId = id
       });
        _context.SaveChanges();
    }

    public void UnFollow(int v, int id)
    {
       _context.Set<Follow>().Remove(new Follow()
       {
           FollowerId = v,
           FolloweeId = id
       });
       _context.SaveChanges();
    }

    public int? GetCountNumberFollower(int id)
    {
        return _context.Set<Follow>().Where(x => x.FolloweeId == id).Count();
    }

    public int? GetCountNumberFollowing(int id)
    {
        return _context.Set<Follow>().Where(x => x.FollowerId == id).Count();
    }

	public void resetPassword(User user)
	{
		User user1 = _context.Users.Where(x=> x.UserId==user.UserId).FirstOrDefault();
        user1.Password = user.Password;
        _context.SaveChanges();
	}
}