using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface IHomePageService
    {
        List<User> GetAllFriendsOfUser();

        Task<IList<UserFriend>> GetAllFriendRequestUser(int userid = -1);
        
        Task<IList<UserFriend>> GetAllFriendRequestUserOther(int userId);
        List<User> GetUpComingBirthdayFriends();
        Task<IList<MessageData>> GetUserChatWith();
        
    }
}
