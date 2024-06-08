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

        public List<User> GetUpComingBirthdayFriends()
        {
            var listFriends = GetAllFriendsOfUser();
            listFriends = listFriends.Where(x => IsUpComingBirthday(x.Dob)).ToList();
            return listFriends;
        }

        private bool IsUpComingBirthday(DateTime? dob)
        {
            var today = DateTime.Now;
            var oneMonthFromToday = today.AddMonths(1);
            var birthday = (DateTime)dob;
            if(birthday.Month == today.Month && birthday.Day >= today.Day)
            {
                return true;
            }
            if (birthday.Month == oneMonthFromToday.Month && birthday.Day <= oneMonthFromToday.Day)
            {
                return true;
            }
            return false;
        }
    }
}
