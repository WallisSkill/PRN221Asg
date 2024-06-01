using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;

        public User ExistUser (string username, string password)
        {
            var user = _userRepository.GetUser(username, password);
            return user;
        }
    }
}
