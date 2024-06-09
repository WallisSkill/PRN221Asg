using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Repository;
[Service]
[RequiredArgsConstructor]
public partial class MessageRepository: IMessageRepository
{
    private readonly social_mediaContext _context;
    public async Task<IList<Message>> GetMessagesForReceiverAsync(string senderId,string receiverId)
    {
        return await _context.Messages
            .Where(x =>
                (x.SenderId == Int32.Parse(senderId) || x.SenderId == Int32.Parse(receiverId))
                &&
                (x.ReceiverId == Int32.Parse(senderId) || x.ReceiverId == Int32.Parse(receiverId))
                )
            .ToListAsync();
    }
}