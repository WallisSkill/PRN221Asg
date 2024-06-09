using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.AspNetCore.SignalR;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Hubs;

[RequiredArgsConstructor]
[Service]
public partial class ChatHub : Hub
{
    private readonly social_mediaContext _context;
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

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }
    }
  
}