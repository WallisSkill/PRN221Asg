using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Data;
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

    public List<MessageData> GetAllMessage(int userId)
    {
        var query = from T1 in _context.Set<Message>()
                    select new MessageData()
                    {
                        SenderId = T1.SenderId,
                        ReceiverId = T1.ReceiverId,
                        Time = (DateTime)T1.SendAt,
                        IsSendedByUser = (T1.SenderId == userId),
                        Message = T1.Content
                    };
        return query.ToList();
    }
}