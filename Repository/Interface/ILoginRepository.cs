using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface ILoginRepository
    {
        User? GetUser(string username, string password);
    }
}
