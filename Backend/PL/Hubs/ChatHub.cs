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
                // Save message
                var savedMessage = await _chatService.SendMessageAsync(senderId, messageVm);

                // Prepare message for receiver
                var receiverMessage = new MessageVM
                {
                    Id = savedMessage.Id,
                    SenderId = savedMessage.SenderId,
                    ReceiverId = savedMessage.ReceiverId,
                    Content = savedMessage.Content,
                    Timestamp = savedMessage.Timestamp,
                    IsRead = false,
                    SenderName = savedMessage.SenderName,
                    ReceiverName = savedMessage.ReceiverName,
                    SenderProfileImg = savedMessage.SenderProfileImg,
                    ReceiverProfileImg = savedMessage.ReceiverProfileImg,
                    IsOwnMessage = false
                };

                // Send to receiver if online
                if (_userConnections.TryGetValue(messageVm.ReceiverId, out var receiverConnectionId))
                {
                    await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", receiverMessage);
                    Console.WriteLine($"Message sent to receiver {messageVm.ReceiverId}");
                }
                else
                {
                    Console.WriteLine($"Receiver {messageVm.ReceiverId} is offline");
                }

                // Send confirmation to sender
                await Clients.Caller.SendAsync("MessageSent", savedMessage);
                Console.WriteLine($"Message sent from {senderId} to {messageVm.ReceiverId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                await Clients.Caller.SendAsync("MessageError", "Failed to send message");
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
