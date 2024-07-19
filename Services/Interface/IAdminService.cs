using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface IAdminService
    {
        List<Friend> GetBlockedAccount();


        List<string> GetAllBadWords();

        List<UserProfile> GetUsers();
        void UpdateLock(int userId, bool v);
    }
}
