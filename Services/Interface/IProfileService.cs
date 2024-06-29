using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface IProfileService
{
    User? GetUserInfo(int userid);
    List<Photo> GetUserPhoto(int userid);
    
    int GetCountNumberLikes(int userid);
    int GetCountNumberComments(int userid);

    List<UserProfile> GetAllFriendOfUser(int userid);
    List<Friend> GetAllFriendRelatetionshipOfUser(int userId);

    void EditProfile(User user);
	Boolean? CheckIsFollow(int currentUserId,int id);
}