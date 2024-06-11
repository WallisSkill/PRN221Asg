using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface;

public interface IMessageService
{
    Task<IList<Message>> GetMessagesForReceiverAsync(string senderId, string receiverId);
    Task UpdateStatusOfMessage(string senderId, string receiverId);
}