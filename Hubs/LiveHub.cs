using System.Text;
using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.AspNetCore.SignalR;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Hubs;

[RequiredArgsConstructor]
[Service]
public partial class LiveHub : Hub
{
    private readonly social_mediaContext _context;
    private readonly IFriendService _friendService;
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

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage-"+receiverId, senderId, message, _context.Users.FirstOrDefault(x=>x.UserId== Int32.Parse(senderId))?.Fullname);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }
    }
    
    public async Task SendFriendRequest(string userId, string friendUserId,string name)
    {
        _friendService.SendFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.Others.SendAsync("ReceiveFriendRequest-"+friendUserId, userId, friendUserId,name);
    }
    public async Task AcceptFriendRequest(string userId, string friendUserId,string name)
    {
        _friendService.AcceptFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.Others.SendAsync("AcceptFriendRequest-"+friendUserId, userId, friendUserId,name);
    }

    public void CancelFriendRequest(string userId, string friendUserId)
    {
        _friendService.CancelFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
    }
    
}