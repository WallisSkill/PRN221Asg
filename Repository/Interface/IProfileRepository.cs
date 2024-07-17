using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface IProfileRepository
{
    User? GetUserInfo(int userid);
    List<Photo> GetUserPhoto(int userid);
    int GetCountNumberLikes(int userid);
    int GetCountNumberComments(int userid);

    void EditProfile(User user);
	bool? CheckIsFollow(int currentUserId,int id);
    void Follow(int v, int id);
    void UnFollow(int v, int id);
    int? GetCountNumberFollower(int id);
    int? GetCountNumberFollowing(int id);
	void resetPassword(User user);
}