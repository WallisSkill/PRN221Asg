using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;

[Service]
[RequiredArgsConstructor]
public partial class ProfileService: IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IFriendRepository _friendRepository;
    public User? GetUserInfo(int userid)
    {
        return _profileRepository.GetUserInfo(userid);
    }

    public List<Photo> GetUserPhoto(int userid)
    {
        return _profileRepository.GetUserPhoto(userid);
    }

    public int GetCountNumberLikes(int userid)
    {
        return _profileRepository.GetCountNumberLikes(userid);
    }

    public int GetCountNumberComments(int userid)
    {
        return _profileRepository.GetCountNumberComments(userid);
    }

    public List<User> GetAllFriendOfUser(int userid)
    {
        var allFriendsRelationshipOfUser = _friendRepository.GetAllFriendRelatetionshipOfUser(userid,true);
        var listFriendId = new List<int>();
        allFriendsRelationshipOfUser.ForEach(item =>
        {
            listFriendId.Add(item.User1Id == userid ? item.User2Id : item.User1Id);
        });
        var listFriends = _friendRepository.GetFriendsOfUser(listFriendId);
        return listFriends;
    }

    public List<Friend> GetAllFriendRelatetionshipOfUser(int userId)
    {
        return _friendRepository.GetAllFriendRelatetionshipOfUser(userId,false);
    }
}