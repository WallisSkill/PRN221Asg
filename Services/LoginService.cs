using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Repository;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class LoginService : ILoginService
    {
        private readonly UserRepository _userRepository;


    }
}
