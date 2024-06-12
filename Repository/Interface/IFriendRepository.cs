using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface IFriendRepository
    {
        List<Friend> GetAllFriendRelatetionshipOfUser(int userId,bool open);
        List<User> GetFriendsOfUser(List<int> userIds);
    }
}
