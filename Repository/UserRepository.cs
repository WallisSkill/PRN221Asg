using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository
{
    [Service]
    [RequiredArgsConstructor]
    public partial class UserRepository : IUserRepository
    {
        private readonly social_mediaContext _context;

        public User? GetUser(string username, string password)
        {
            var query = _context.Set<User>().FirstOrDefault(x => x.Username == username && x.Password == password);
            return query;
        }
    }
}
