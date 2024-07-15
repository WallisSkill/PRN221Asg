using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Newtonsoft.Json;
using PRN221_Assignment.ExtenModel;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository
{
    [Service]
    [RequiredArgsConstructor]
    public partial class LoginRepository : ILoginRepository
    {
        private readonly social_mediaContext _context;

        public User? GetUser(string username, string password)
        {
            var query = _context.Set<User>().FirstOrDefault(x => x.Username == username && x.Password == password);
            return query;
        }

        public Admin? GetUserAdmin(string? username, string? key)
        {
            return GetAdmins().FirstOrDefault(x => x.Username == username && x.Key == key);
        }

        private List<Admin>? GetAdmins() {
            string filePath = "Admin.json"; // Đường dẫn tới file Admin.json

            // Đọc nội dung file Admin.json
            string json = File.ReadAllText(filePath);

            // Parse JSON thành danh sách các đối tượng Admin
            return JsonConvert.DeserializeObject<List<Admin>>(json);
        }
    }
}
