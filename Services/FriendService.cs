using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;
[Service]
public partial class FriendService: IFriendService
{
    private readonly IFriendRepository _friendRepository;
    private readonly IPostRepository _postRepository;
    private readonly int _currentUserId;

    public FriendService(IFriendRepository friendRepository, IUserResolverService userResolver,
    IPostRepository postRepository)
    {
        _friendRepository = friendRepository;
        _currentUserId = userResolver.GetUser();
        _postRepository = postRepository;
    }

    public void SendFriendRequest(int userId,int receiverId)
    {
       _friendRepository.SendFriendRequest(userId,receiverId);
    }

    public void CancelFriendRequest(int userId, int receiverId)
    {
        _friendRepository.CancelFriendRequest(userId,receiverId);
    }

    public void AcceptFriendRequest(int userId, int receiverId)
    {
        _friendRepository.AcceptFriendRequest(userId,receiverId);
    }

    public List<User> GetSuggestfriend()
    {
        var listFriend = _postRepository.GetAllFriendId(_currentUserId);
        var listSuggestId = new List<int>();
        listFriend.ForEach(item =>
        {
            listSuggestId.AddRange(_postRepository.GetAllFriendId(item));
        });
        listSuggestId = listSuggestId.Distinct().ToList();

        var listRelation = _friendRepository.GetAllFriendRelationshipsOfUser(_currentUserId, false);
        listRelation.ForEach(x =>
        {
            if(listSuggestId.Any(c => c == x.User1Id))
            {
                listSuggestId.Remove(x.User1Id);
            }
            if(listSuggestId.Any(c => c == x.User2Id))
            {
                listSuggestId.Remove(x.User2Id);
            }
        });

        var suggestFriends = _friendRepository.GetFriendsOfUser(listSuggestId);
        return suggestFriends.Take(5).ToList();
    }
}