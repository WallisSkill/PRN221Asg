using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface IFriendRepository
    {
        List<Friend> GetAllFriendRelationshipsOfUser(int userId,bool open);

        Task<IList<Friend>> GetAllFriendRequestUser(int userId);
        
        Task<IList<Friend>> GetAllFriendRequestUserOther(int userId);
        

        Task<IList<UserFriend>> GetFriendsOfUserAsync(List<int> userIds,int currentUserId);
        
        Task<IList<UserFriend>> GetFriendsOfUserAsyncOther(List<int> userIds,int currentUserId);
        List<User> GetFriendsOfUser(List<int> userIds);
        void SendFriendRequest(int userId,int receiverId);
        void CancelFriendRequest(int userId, int receiverId);
        void AcceptFriendRequest(int userId, int receiverId);
    }
}
