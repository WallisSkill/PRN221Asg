using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface IProfileRepository
{
    User? GetUserInfo(int userid);
    List<Photo> GetUserPhoto(int userid);
}