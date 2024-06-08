using DependencyInjectionAutomatic.Service;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;




namespace PRN221_Assignment.Services
{
    [Service]
    public partial class HomePageService : IHomePageService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly int _currentUserId;

        public HomePageService(IFriendRepository friendRepository, IUserResolverService userResolver)
        {
            _friendRepository = friendRepository;
            _currentUserId = userResolver.GetUser();
        }

        public List<User> GetAllFriendsOfUser()
        {
            var listReletionship = _friendRepository.GetAllFriendRelatetionshipOfUser(_currentUserId);
            var listFriendId = new List<int>();
            listReletionship.ForEach(item =>
            {
                if (item.User1Id == _currentUserId)
                {
                    listFriendId.Add(item.User2Id);
                }
                else
                {
                    listFriendId.Add(item.User1Id);
                }
            });
            var listFriends = _friendRepository.GetFriendsOfUser(listFriendId);
            return listFriends;
        }
    }
}
