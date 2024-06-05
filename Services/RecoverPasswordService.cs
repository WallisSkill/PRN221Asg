using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class RecoverPasswordService : IRecoverPasswordService
    {
        private readonly ISignUpRepository _signUpRepository;

        public User GetUserByMail(string email)
        {
            return _signUpRepository.GetUsers().FirstOrDefault(x => x.Email == email);
        }
    }
}
