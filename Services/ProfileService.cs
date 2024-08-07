using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;

[Service]
[RequiredArgsConstructor]
public partial class ProfileService : IProfileService
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

    public List<UserProfile> GetAllFriendOfUser(int userid)
    {
        var allFriendsRelationshipOfUser = _friendRepository.GetAllFriendRelationshipsOfUser(userid,true);
        var listFriendId = new List<int>();
        allFriendsRelationshipOfUser.ForEach(item =>
        {
            listFriendId.Add(item.User1Id == userid ? item.User2Id : item.User1Id);
        });
        var listFriends = _friendRepository.GetFriendsOfUser(listFriendId);
        var listFriendsCount = new List<UserProfile>();
        foreach (var friend in listFriends)
        {
            listFriendsCount.Add(new UserProfile()
            {
               Count = _friendRepository.GetAllFriendRelationshipsOfUser(friend.UserId, true).Count,
               User = friend
            });
        }
        return listFriendsCount;
    }

    public List<Friend> GetAllFriendRelatetionshipOfUser(int userId)
    {
        return _friendRepository.GetAllFriendRelationshipsOfUser(userId,false);
    }

    public void EditProfile(User user)
    {
        _profileRepository.EditProfile(user);
    }

	public bool? CheckIsFollow(int currentUserId,int id)
	{
		return _profileRepository.CheckIsFollow(currentUserId,id);
	}

    public void Follow(int v, int id)
    {
        _profileRepository.Follow(v, id);
    }

    public void UnFollow(int v, int id)
    {
        _profileRepository.UnFollow(v, id);
    }

    public int? GetCountNumberFollower(int id)
    {
        return _profileRepository.GetCountNumberFollower(id);
    }

    public int? GetCountNumberfollowing(int id)
    {
        return _profileRepository.GetCountNumberFollowing(id);
    }

	public void resetPassword(User user)
	{
        _profileRepository.resetPassword(user);
	}
}