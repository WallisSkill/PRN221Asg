using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class SignUpService : ISignUpService
    {
        private readonly ISignUpRepository _signUpRepository;

        public bool CheckUsernameExist(string username)
        {
            var users = _signUpRepository.GetUsers();
            return users.Any(u => u.Username == username);
        }

        public bool CheckEmailExist(string email)
        {
            var users = _signUpRepository.GetUsers();
            return users.Any(u => u.Email == email);
        }

        public void InsertUser(User user) 
        { 
            _signUpRepository.InsertUser(user);
        }
    }
}
