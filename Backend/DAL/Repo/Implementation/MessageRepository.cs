//Team Member: Abdallah Assem
// Date: 14-11-2025
// Descriprion: update MessageRepository to include chat-specific methods
namespace DAL.Repo.Implementation
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {

        public MessageRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Message>> GetConversationAsync(Guid userId1, Guid userId2)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m =>
                    (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                    (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(Guid receiverId)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == receiverId && !m.IsRead)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }
        public async Task<Message> CreateAsync(Message message)
        {
            _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<Message> GetByIdAsync(int id)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.Update(message.Content, true);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> GetUnreadCountAsync(Guid receiverId)
        {
            return await _context.Messages
                .CountAsync(m => m.ReceiverId == receiverId && !m.IsRead);
        }
        public async Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

        }
    }
}
