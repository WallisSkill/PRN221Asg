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
    private readonly social_mediaContext _context;
    private readonly IFriendRepository friendRepository;
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
        friendRepository.SendFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.Others.SendAsync("ReceiveFriendRequest-"+friendUserId, userId, friendUserId,name);
    }
    public async Task AcceptFriendRequest(string userId, string friendUserId,string name)
    {
        friendRepository.AcceptFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.Others.SendAsync("AcceptFriendRequest-"+friendUserId, userId, friendUserId,name);
    }

    public void CancelFriendRequest(string userId, string friendUserId)
    {
        friendRepository.CancelFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
    }
    
}