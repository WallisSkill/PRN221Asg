using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface IHomePageService
    {
        List<User> GetAllFriendsOfUser();

        Task<IList<User>> GetAllFriendRequestUser();
        List<User> GetUpComingBirthdayFriends();
        Task<IList<MessageData>> GetUserChatWith();
        
    }
}
