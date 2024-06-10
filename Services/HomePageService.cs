using DependencyInjectionAutomatic.Service;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;




namespace PRN221_Assignment.Services
{
    [Service]
    public partial class HomePageService : IHomePageService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMessageRepository _messageRepository;
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

        public List<MessageData> GetUserChatWith()
        {
            var listData = new List<MessageData>();
            var listMessage = _messageRepository.GetAllMessage(_currentUserId);
            var listMessageLater = listMessage.GroupBy(x => new { x.SenderId, x.ReceiverId }).Select(x => new MessageData()
            {
                SenderId = x.Key.SenderId,
                ReceiverId = x.Key.ReceiverId,
                Message = x.MinBy(x => x.Time).Message,
                Time = x.Min(x => x.Time),
                IsSendedByUser = x.MinBy(x => x.Time).IsSendedByUser,
            }).ToList();
            listData.AddRange(listMessageLater);
            listMessageLater.ForEach(item =>
            {
                var message = new MessageData()
                {
                    SenderId = item.ReceiverId,
                    ReceiverId = item.SenderId,
                    Message = item.Message,
                    Time = item.Time,
                    IsSendedByUser = item.IsSendedByUser,
                };
                listData.Add(message);
            });
            listData.RemoveAll(x => x.SenderId != _currentUserId);
            listData = listData.GroupBy(x => new { x.SenderId, x.ReceiverId }).Select(x => new MessageData()
            {
                SenderId = x.Key.SenderId,
                ReceiverId = x.Key.ReceiverId,
                Message = x.MinBy(x => x.Time).Message,
                Time = x.Min(x => x.Time),
                IsSendedByUser = x.MinBy(x => x.Time).IsSendedByUser,
            }).OrderBy(x => x.Time).ToList();
            return listData;
        }
    }
}
