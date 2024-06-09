using Lombok.NET;
using Microsoft.AspNetCore.SignalR;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Hubs;

[RequiredArgsConstructor]
public partial class ChatHub : Hub
{
    private readonly social_mediaContext _context;
    
    
    public async Task SendMessage(int senderId, int receiverId, string message)
    {
        var newMessage = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = message,
            SendAt = DateTime.Now,
            IsRead = false
        };

        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();

        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message);
    }
  
}