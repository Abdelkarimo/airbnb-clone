//Team Member: Abdallah Assem
// Date: 14-11-2025
// Descriprion: update IMessageRepository to include chat-specific methods
namespace DAL.Repo.Abstraction
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<Message> CreateAsync(Message message);
        Task<Message> GetByIdAsync(int id);
        Task MarkAsReadAsync(int messageId);
        Task<int> GetUnreadCountAsync(Guid receiverId);
        Task<IEnumerable<Message>> GetConversationAsync(Guid userId1, Guid userId2);     // Chat between 2 users
        Task<IEnumerable<Message>> GetUnreadMessagesAsync(Guid receiverId);              // Unread messages for a receiver
        Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId); // ADD THIS
    }
}