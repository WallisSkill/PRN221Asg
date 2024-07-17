using DependencyInjectionAutomatic.Service;
using Newtonsoft.Json;
using PRN221_Assignment.ExtenModel;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository
{
    [Service]
    public class AdminRepository : IAdminRepository
    {
        public List<int> GetBlockedAccount()
        {
            string filePath = "AdminJson/Admin.json";

            // Đọc nội dung file Admin.json
            string json = File.ReadAllText(filePath);

            var adminData = JsonConvert.DeserializeObject<AdminData>(json);

            return adminData?.BlockedAccount;
        }

        public void AddBlockedAccount(int newId)
        {
            string filePath = "AdminJson/Admin.json";

            string json = File.ReadAllText(filePath);

            var adminData = JsonConvert.DeserializeObject<AdminData>(json);

            if (adminData != null)
            {
                adminData.BlockedAccount.Add(newId);

                string updatedJson = JsonConvert.SerializeObject(adminData, Formatting.Indented);

                File.WriteAllText(filePath, updatedJson);
            }
        }

        public List<string> GetAllBadWords()
        {
            string filePath = "AdminJson/badword.json";

            // Đọc nội dung file Admin.json
            string json = File.ReadAllText(filePath);

            var adminData = JsonConvert.DeserializeObject<BadWords>(json);

            return adminData?.words;
        }

        public void RemoveBlockedAccount(int userId)
        {
            string filePath = "AdminJson/Admin.json";

            string json = File.ReadAllText(filePath);

            var adminData = JsonConvert.DeserializeObject<AdminData>(json);

            if (adminData != null)
            {
                adminData.BlockedAccount.Remove(userId);

                string updatedJson = JsonConvert.SerializeObject(adminData, Formatting.Indented);

                File.WriteAllText(filePath, updatedJson);
            }
        }
    }
}
