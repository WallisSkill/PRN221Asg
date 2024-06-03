using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface ISignUpRepository
    {
        List<User> GetUsers();
        void InsertUser(User user);
    }
}
