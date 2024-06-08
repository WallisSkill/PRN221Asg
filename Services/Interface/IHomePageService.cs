using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface IHomePageService
    {
        List<User> GetAllFriendsOfUser();
        List<User> GetUpComingBirthdayFriends();
    }
}
