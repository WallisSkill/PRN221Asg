using DependencyInjectionAutomatic.Service;
using Lombok.NET;
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
        public List<Friend> GetAllFriendRelatetionshipOfUser (int userId,bool open= true)
        {
            var query = _context.Set<Friend>().Where(x => (x.User1Id == userId || x.User2Id == userId));
            if (open) query = query.Where(x => x.Status == true);
            return query.ToList();
        }

        public List<User> GetFriendsOfUser(List<int> userIds)
        {
            var query = _context.Set<User>().Where(x => userIds.Contains(x.UserId));
            return query.ToList();
        }
    }
}
