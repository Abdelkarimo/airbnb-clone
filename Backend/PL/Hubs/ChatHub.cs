using BLL.ModelVM.Chat;
using BLL.Services;
using BLL.Services.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace PL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private static readonly Dictionary<Guid, string> _userConnections = new();

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task RegisterUser(Guid userId)
        {
            _userConnections[userId] = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            await Clients.Caller.SendAsync("UserRegistered", true);

            Console.WriteLine($"User {userId} registered with connection {Context.ConnectionId}");
        }

        public async Task SendMessage(Guid senderId, CreateMessageVM messageVm)
        {
            try
            {
                Console.WriteLine($"SendMessage called - Sender: {senderId}, Receiver: {messageVm.ReceiverId}");

                // 1. Save message to database
                var savedMessage = await _chatService.SendMessageAsync(senderId, messageVm);
                Console.WriteLine($"Message saved with ID: {savedMessage.Id}");

                // 2. Get user details for proper display
                var sender = await _chatService.GetUserByIdAsync(senderId);
                var receiver = await _chatService.GetUserByIdAsync(messageVm.ReceiverId);

                // 3. Prepare message for RECEIVER
                var receiverMessage = new MessageVM
                {
                    Id = savedMessage.Id,
                    SenderId = savedMessage.SenderId,
                    ReceiverId = savedMessage.ReceiverId,
                    Content = savedMessage.Content,
                    SentAt = savedMessage.SentAt,
                    IsRead = savedMessage.IsRead,
                    SenderName = sender?.FullName ?? "Unknown User",
                    ReceiverName = receiver?.FullName ?? "Unknown User",
                    SenderProfileImg = sender?.ProfileImg ?? "",
                    ReceiverProfileImg = receiver?.ProfileImg ?? "",
                    IsOwnMessage = false // Receiver sees this as someone else's message
                };

                // 4. Send to receiver if online
                if (_userConnections.TryGetValue(messageVm.ReceiverId, out var receiverConnectionId))
                {
                    await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", receiverMessage);
                    Console.WriteLine($"Message delivered to receiver: {messageVm.ReceiverId}");
                }
                else
                {
                    Console.WriteLine($"Receiver offline: {messageVm.ReceiverId}");
                }

                // 5. Send confirmation to SENDER
                var senderMessage = new MessageVM
                {
                    Id = savedMessage.Id,
                    SenderId = savedMessage.SenderId,
                    ReceiverId = savedMessage.ReceiverId,
                    Content = savedMessage.Content,
                    SentAt = savedMessage.SentAt,
                    IsRead = savedMessage.IsRead,
                    SenderName = "You", // Sender sees themselves as "You"
                    ReceiverName = receiver?.FullName ?? "Unknown User",
                    SenderProfileImg = sender?.ProfileImg ?? "",
                    ReceiverProfileImg = receiver?.ProfileImg ?? "",
                    IsOwnMessage = true // Sender sees this as their own message
                };

                await Clients.Caller.SendAsync("MessageSent", senderMessage);
                Console.WriteLine($"Message sent confirmation to sender: {senderId}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessage: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                await Clients.Caller.SendAsync("MessageError", $"Failed to send message: {ex.Message}");
            }
        }

        public async Task MarkConversationAsRead(Guid currentUserId, Guid otherUserId)
        {
            await _chatService.MarkMessagesAsReadAsync(currentUserId, otherUserId);

            // Notify the other user that messages were read
            if (_userConnections.TryGetValue(otherUserId, out var otherUserConnectionId))
            {
                await Clients.Client(otherUserConnectionId).SendAsync("MessagesRead", currentUserId);
            }
        }

        public async Task GetConversation(Guid currentUserId, Guid otherUserId)
        {
            var messages = await _chatService.GetConversationAsync(currentUserId, otherUserId);
            await Clients.Caller.SendAsync("ConversationLoaded", messages);
        }

        public async Task GetUserConversations(Guid userId)
        {
            var conversations = await _chatService.GetUserConversationsAsync(userId);
            await Clients.Caller.SendAsync("ConversationsLoaded", conversations);
        }

        public async Task GetUnreadCount(Guid userId)
        {
            var count = await _chatService.GetUnreadCountAsync(userId);
            await Clients.Caller.SendAsync("UnreadCountLoaded", count);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != Guid.Empty)
            {
                _userConnections.Remove(userId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
                Console.WriteLine($"User {userId} disconnected");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}