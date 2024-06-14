using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;
[Service]
[RequiredArgsConstructor]
public partial class FriendService: IFriendService
{
    private readonly IFriendRepository _friendRepository;
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
}