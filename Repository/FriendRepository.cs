using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Repository
{
    [Service]
    [RequiredArgsConstructor]
    public partial class FriendRepository : IFriendRepository
    {
        private readonly social_mediaContext _context;
        public List<Friend> GetAllFriendRelationshipsOfUser (int userId,bool open= true)
        {
            var query = _context.Set<Friend>().Where(x => (x.User1Id == userId || x.User2Id == userId));
            if (open) query = query.Where(x => x.Status == true);
            return query.ToList();
        }
        
        public async Task<IList<Friend>> GetAllFriendRequestUser (int userId)
        {
            DateTime twoMinutesAgo = DateTime.Now.AddMinutes(-2);
            var query = _context.Set<Friend>().Where(x =>  x.User2Id == userId&&(x.Status ==false || (x.Status == true && x.CreatedAt >= twoMinutesAgo))).ToListAsync();
            return await query;
        }

        public async Task<IList<Friend>> GetAllFriendRequestUserOther(int userId)
        {
            DateTime twoMinutesAgo = DateTime.Now.AddMinutes(-2);
            var query = _context.Set<Friend>().Where(x =>  x.User1Id == userId&&((x.Status == true && x.CreatedAt >= twoMinutesAgo))).ToListAsync();
            return await query;
        }

        public async Task<IList<UserFriend>> GetFriendsOfUserAsync(List<int> userIds,int currentUserId)
        {
            DateTime twoMinutesAgo = DateTime.Now.AddMinutes(-2);
            var query = from u in _context.Set<User>()
                join f in _context.Set<Friend>() on u.UserId equals f.User1Id into ufGroup
                from f in ufGroup.DefaultIfEmpty()
                where userIds.Contains(f.User1Id) && f.User2Id == currentUserId && (f.Status == false || (f.Status == true && f.CreatedAt >= twoMinutesAgo))
                select new UserFriend
                {
                    User = u,
                    Status = f.Status
                };
            return await query.ToListAsync();
        }

        public async Task<IList<UserFriend>> GetFriendsOfUserAsyncOther(List<int> userIds, int currentUserId)
        {
            DateTime twoMinutesAgo = DateTime.Now.AddMinutes(-2);
            var query = from u in _context.Set<User>()
                join f in _context.Set<Friend>() on u.UserId equals f.User2Id into ufGroup
                from f in ufGroup.DefaultIfEmpty()
                where userIds.Contains(f.User2Id) && f.User1Id == currentUserId && ((f.Status == true && f.CreatedAt >= twoMinutesAgo))
                select new UserFriend
                {
                    User = u,
                    Status = f.Status
                };
            return await query.ToListAsync();
        }

        public List<User> GetFriendsOfUser(List<int> userIds)
        {
            var query = _context.Set<User>().Where(x => userIds.Contains(x.UserId));
            return query.ToList();
        }

        public void SendFriendRequest(int userId, int receiverId)
        {
            Friend newRequest = new Friend()
            {
                User1Id = userId,
                User2Id = receiverId,
                CreatedAt = DateTime.Now,
                Status = false
            };
            _context.Friends.Add(newRequest);
            _context.SaveChanges();
        }

        public void CancelFriendRequest(int userId, int receiverId)
        {
            var friend = _context.Set<Friend>().FirstOrDefault(x => (x.User1Id == userId && x.User2Id == receiverId) || (x.User1Id == receiverId && x.User2Id == userId));
            if (friend == null) return;
            _context.Set<Friend>().Remove(friend);
            _context.SaveChanges();
        }

        public void AcceptFriendRequest(int userId, int receiverId)
        {
            var friend = _context.Set<Friend>().FirstOrDefault(x => (x.User1Id == userId && x.User2Id == receiverId) || (x.User1Id == receiverId && x.User2Id == userId));
            if (friend == null) return;
            friend.Status = true;
            _context.Set<Friend>().Update(friend);
            _context.SaveChanges();
        }
    }
}
