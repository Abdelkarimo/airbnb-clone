
using BLL.ModelVM.Chat;
using BLL.Services.Abstractions;

namespace BLL.Services.Impelementation
{
    public class ChatService : IChatService
    {
        private readonly List<MessageVM> _mockMessages = new();
        private readonly List<UserVM> _mockUsers = new();
        private int _messageIdCounter = 1;

        public ChatService()
        {
            InitializeMockData();
        }

        private void InitializeMockData()
        {
            string guidString = "a1b2c3d4-e5f6-7890-1234-567890abcdef";
            // Create mock users
            var user1 = new UserVM
            {

                Id = Guid.Parse(guidString),
                FullName = "John Doe",
                Email = "john@example.com",
                Role = "Host",
                ProfileImg = "/assets/user1.jpg",
                DateCreated = DateTime.UtcNow.AddDays(-30),
                FirebaseUid = "firebase123",
                IsActive = true
            };

            var user2 = new UserVM
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Smith",
                Email = "jane@example.com",
                Role = "Guest",
                ProfileImg = "/assets/user2.jpg",
                DateCreated = DateTime.UtcNow.AddDays(-15),
                FirebaseUid = "firebase456",
                IsActive = true
            };

            var user3 = new UserVM
            {
                Id = Guid.NewGuid(),
                FullName = "Mike Johnson",
                Email = "mike@example.com",
                Role = "Guest",
                ProfileImg = "/assets/user3.jpg",
                DateCreated = DateTime.UtcNow.AddDays(-10),
                FirebaseUid = "firebase789",
                IsActive = true
            };

            _mockUsers.AddRange(new[] { user1, user2, user3 });

            // Create mock messages
            _mockMessages.Add(new MessageVM
            {
                Id = _messageIdCounter++,
                SenderId = user2.Id,
                ReceiverId = user1.Id,
                Content = "Hello, is this property available next weekend?",
                Timestamp = DateTime.UtcNow.AddHours(-2),
                IsRead = false,
                SenderName = user2.FullName,
                ReceiverName = user1.FullName,
                SenderProfileImg = user2.ProfileImg,
                ReceiverProfileImg = user1.ProfileImg,
                IsOwnMessage = false
            });

            _mockMessages.Add(new MessageVM
            {
                Id = _messageIdCounter++,
                SenderId = user1.Id,
                ReceiverId = user2.Id,
                Content = "Yes, it's available! Would you like to book it?",
                Timestamp = DateTime.UtcNow.AddHours(-1),
                IsRead = true,
                SenderName = user1.FullName,
                ReceiverName = user2.FullName,
                SenderProfileImg = user1.ProfileImg,
                ReceiverProfileImg = user2.ProfileImg,
                IsOwnMessage = false
            });
        }

        public async Task<MessageVM> SendMessageAsync(Guid senderId, CreateMessageVM messageVm)
        {
            var sender = await GetUserByIdAsync(senderId);
            var receiver = await GetUserByIdAsync(messageVm.ReceiverId);

            var newMessage = new MessageVM
            {
                Id = _messageIdCounter++,
                SenderId = senderId,
                ReceiverId = messageVm.ReceiverId,
                Content = messageVm.Content,
                Timestamp = DateTime.UtcNow,
                IsRead = false,
                SenderName = sender?.FullName ?? "Unknown User",
                ReceiverName = receiver?.FullName ?? "Unknown User",
                SenderProfileImg = sender?.ProfileImg ?? "",
                ReceiverProfileImg = receiver?.ProfileImg ?? "",
                IsOwnMessage = true
            };

            _mockMessages.Add(newMessage);
            return newMessage;
        }

        public async Task<List<MessageVM>> GetConversationAsync(Guid currentUserId, Guid otherUserId)
        {
            var messages = _mockMessages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                           (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            foreach (var message in messages)
            {
                message.IsOwnMessage = message.SenderId == currentUserId;
            }

            return await Task.FromResult(messages);
        }

        public async Task<List<ConversationVM>> GetUserConversationsAsync(Guid userId)
        {
            var userMessages = _mockMessages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.Timestamp)
                .ToList();

            var conversations = new Dictionary<Guid, ConversationVM>();

            foreach (var message in userMessages)
            {
                var otherUserId = message.SenderId == userId ? message.ReceiverId : message.SenderId;

                if (!conversations.ContainsKey(otherUserId))
                {
                    var otherUser = await GetUserByIdAsync(otherUserId);
                    var unreadCount = _mockMessages
                        .Count(m => m.ReceiverId == userId && m.SenderId == otherUserId && !m.IsRead);

                    conversations[otherUserId] = new ConversationVM
                    {
                        OtherUserId = otherUserId,
                        OtherUserName = otherUser?.FullName ?? "Unknown User",
                        OtherUserProfileImg = otherUser?.ProfileImg ?? "",
                        LastMessage = message.Content.Length > 50
                            ? message.Content.Substring(0, 50) + "..."
                            : message.Content,
                        LastMessageTime = message.Timestamp,
                        UnreadCount = unreadCount
                    };
                }
            }

            return conversations.Values.OrderByDescending(c => c.LastMessageTime).ToList();
        }

        public async Task MarkMessagesAsReadAsync(Guid currentUserId, Guid otherUserId)
        {
            var messagesToMark = _mockMessages
                .Where(m => m.SenderId == otherUserId && m.ReceiverId == currentUserId && !m.IsRead)
                .ToList();

            foreach (var message in messagesToMark)
            {
                message.IsRead = true;
            }

            await Task.CompletedTask;
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            var count = _mockMessages
                .Count(m => m.ReceiverId == userId && !m.IsRead);

            return await Task.FromResult(count);
        }

        public async Task<UserVM> GetUserByIdAsync(Guid userId)
        {
            var user = _mockUsers.FirstOrDefault(u => u.Id == userId);
            return await Task.FromResult(user);
        }
    }
}
