using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface IProfileService
{
    User? GetUserInfo(int userid);
    List<Photo> GetUserPhoto(int userid);
    
    int GetCountNumberLikes(int userid);
    int GetCountNumberComments(int userid);
}