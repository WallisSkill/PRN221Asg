using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface ISignUpService
    {
        bool CheckEmailExist(string email);
        bool CheckUsernameExist(string username);
        void InsertUser(User user);
    }
}
