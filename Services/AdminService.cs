using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly ISignUpRepository _signUpRepository;


        public List<Friend> GetBlockedAccount()
        {
            var blockedid = _adminRepository.GetBlockedAccount();
            var list = new List<Friend>();
            foreach (var id in blockedid)
            {
                list.Add(new Friend()
                {
                    User1Id = id
                });
            }
           return list;
        }

        public List<string> GetAllBadWords()
        {
            return _adminRepository.GetAllBadWords();
        }

        public List<UserProfile> GetUsers()
        {
            var users = _signUpRepository.GetUsers();
            var list = new List<UserProfile>();
            foreach(var user in users)
            {
                list.Add(new UserProfile
                {
                    User = user,
                    Count = _friendRepository.GetAllFriendRelationshipsOfUser(user.UserId, true).Count
                });
                
           };
            return list;
        }

        public void UpdateLock(int userId, bool isLocked) =>
            (isLocked ? (Action<int>)_adminRepository.AddBlockedAccount : _adminRepository.RemoveBlockedAccount)(userId);

    }
}
