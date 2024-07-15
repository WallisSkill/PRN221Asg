using PRN221_Assignment.ExtenModel;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface ILoginRepository
    {
        User? GetUser(string username, string password);
        Admin? GetUserAdmin(string? username, string? key);
    }
}
