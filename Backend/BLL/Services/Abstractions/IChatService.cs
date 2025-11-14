
using BLL.ModelVM.Chat;

namespace BLL.Services.Abstractions
{
    public interface IChatService
    {
        Task<MessageVM> SendMessageAsync(Guid senderId, CreateMessageVM messageVM);
        Task<List<MessageVM>> GetConversationAsync(Guid currentUserId, Guid otherUserId);
        Task<List<ConversationVM>> GetUserConversationsAsync(Guid userId);
        Task MarkMessagesAsReadAsync(Guid currentUserId, Guid otherUserId);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task<UserVM> GetUserByIdAsync(Guid userId);
    }
}
