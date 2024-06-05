using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface IRecoverPasswordService
    {
        User? GetUserByMail(string email);
    }
}
