using BLL.ModelVM.Chat;
using BLL.Services.Abstractions;
using DAL.Entities;
using DAL.Repo;
using DAL.Repo.Abstraction;

namespace BLL.Services
{
    public class ChatService : IChatService
    {
        private readonly IMessageRepository _messageRepository;

        public ChatService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<MessageVM> SendMessageAsync(Guid senderId, CreateMessageVM messageVm)
        {
            var message = Message.Create(
                senderId: senderId,
                receiverId: messageVm.ReceiverId,
                content: messageVm.Content,
                sentAt: DateTime.UtcNow,
                isRead: false
            );

            var savedMessage = await _messageRepository.CreateAsync(message);

            return new MessageVM
            {
                Id = savedMessage.Id,
                SenderId = savedMessage.SenderId,
                ReceiverId = savedMessage.ReceiverId,
                Content = savedMessage.Content,
                SentAt = savedMessage.SentAt,
                IsRead = savedMessage.IsRead,
                IsOwnMessage = true
            };
        }

        public async Task<List<MessageVM>> GetConversationAsync(Guid currentUserId, Guid otherUserId)
        {
            var messages = await _messageRepository.GetConversationAsync(currentUserId, otherUserId);

            return messages.Select(m => new MessageVM
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead,
                IsOwnMessage = m.SenderId == currentUserId
            }).ToList();
        }

        public async Task<List<ConversationVM>> GetUserConversationsAsync(Guid userId)
        {
            // Return empty list for now - implement later
            return new List<ConversationVM>();
        }

        public async Task MarkMessagesAsReadAsync(Guid currentUserId, Guid otherUserId)
        {
            var unreadMessages = await _messageRepository.GetUnreadMessagesAsync(currentUserId);
            foreach (var message in unreadMessages.Where(m => m.SenderId == otherUserId))
            {
                await _messageRepository.MarkAsReadAsync(message.Id);
            }
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _messageRepository.GetUnreadCountAsync(userId);
        }

        public async Task<UserVM> GetUserByIdAsync(Guid userId)
        {
            // Return dummy user for now
            return new UserVM
            {
                Id = userId,
                FullName = "Test User",
                Email = "test@example.com",
                Role = "Guest"
            };
        }
    }
}