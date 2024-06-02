using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface IUserRepository
    {
        User? GetUser(string username, string password);
    }
}
