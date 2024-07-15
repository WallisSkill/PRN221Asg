using PRN221_Assignment.ExtenModel;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface ILoginService
    {
        User? ExistUser(string username, string password);
        Admin ExistUserAdmin(string? username, string? key);
    }
}
