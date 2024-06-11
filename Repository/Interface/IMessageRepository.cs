using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface;

public interface IMessageRepository
{
    Task<IList<Message>> GetMessagesForReceiverAsync(string senderId,string receiverId);
    Task<IList<MessageData>> GetAllMessage(int userId);
    Task<IList<User>> GetUsers();
}