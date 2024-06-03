using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository
{
    [Service]
    [RequiredArgsConstructor]
    public partial class SignUpRepository : ISignUpRepository
    {
        private readonly social_mediaContext _context;

        public List<User> GetUsers()
        {
            return _context.Set<User>().ToList();
        }

        public void InsertUser(User user)
        {
            _context.Set<User>().Add(user);
            _context.SaveChanges();
        }
    }
}
