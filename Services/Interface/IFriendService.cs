using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface IFriendService
{
    void SendFriendRequest(int userId, int receiverId);

    void CancelFriendRequest(int userId, int receiverId);

    void AcceptFriendRequest(int userId, int receiverId);

    List<User> GetSuggestfriend();
}