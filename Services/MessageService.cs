using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;

[Service]
[RequiredArgsConstructor]
public partial class MessageService: IMessageService
{
    private readonly IMessageRepository _messageRepository;
    public async Task<IList<Message>> GetMessagesForReceiverAsync(string senderId,string receiverId)
    {
        return await _messageRepository.GetMessagesForReceiverAsync(senderId, receiverId);
    }

    public async Task UpdateStatusOfMessage(string  senderId,string receiverId)
    {
        await _messageRepository.UpdateMessage(senderId, receiverId);
    }
}