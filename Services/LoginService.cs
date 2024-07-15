using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.ExtenModel;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public User? ExistUser (string username, string password)
        {
            var user = _loginRepository.GetUser(username, password);
            return user;
        }

        public Admin ExistUserAdmin(string? username, string? key)
        {
            var user = _loginRepository.GetUserAdmin(username, key);
            return user;
        }
    }
}
