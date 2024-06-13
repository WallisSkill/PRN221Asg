using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface IFriendRepository
    {
        List<Friend> GetAllFriendRelatetionshipOfUser(int userId,bool open);

        Task<IList<Friend>> GetAllFriendRequestUser(int userId);

        Task<IList<User>> GetFriendsOfUserAsync(List<int> userIds);
        List<User> GetFriendsOfUser(List<int> userIds);
        void SendFriendRequest(int userId,int receiverId);
        void CancelFriendRequest(int userId, int receiverId);
    }
}
