using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text;
using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.AspNetCore.SignalR;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Hubs;

[RequiredArgsConstructor]
public partial class LiveHub : Hub
{
    private readonly IFriendRepository _friendRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IProfileRepository _profileRepository;
    public async Task SendMessage(string senderId, string receiverId, string message)
    {
        try
        {
            var newMessage = new Message
            {
                SenderId = Int32.Parse(senderId),
                ReceiverId = Int32.Parse(receiverId),
                Content = message,
                SendAt = DateTime.Now,
                IsRead = false
            };
          await _messageRepository.InsertNewMessage(newMessage);

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message, _profileRepository.GetUserInfo(Int32.Parse(senderId))?.Fullname);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }
    }
    
    public async Task SendFriendRequest(string userId, string friendUserId,string name)
    {
        _friendRepository.SendFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.User(friendUserId).SendAsync("ReceiveFriendRequest", userId, friendUserId,name);
    }
    public async Task AcceptFriendRequest(string userId, string friendUserId,string name)
    {
        _friendRepository.AcceptFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.User(friendUserId).SendAsync("AcceptFriendRequest", userId, friendUserId,name);
    }

    public void CancelFriendRequest(string userId, string friendUserId)
    {
        _friendRepository.CancelFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
    }
    
    
    //status online
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static ConcurrentDictionary<string, string> _users = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = Context.User.FindFirstValue(ClaimTypes.GivenName);

        if (userId != null)
        {
            _users.TryAdd(userId,Context.ConnectionId);
            await Clients.All.SendAsync("UserConnected", userId, username);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        _users.TryRemove(userId, out _);
        await Clients.All.SendAsync("UserDisconnected", userId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetOnlineUsers()
    {
        var onlineUsers = _users.Keys.ToList();
        await Clients.All.SendAsync("ReceiveOnlineUsers", onlineUsers);
    }
    
    public bool IsUserOnline(string userId)
    {
        return _users.ContainsKey(userId);
    }
}