
namespace BLL.ModelVM.Chat
{
    public class MessageVM
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }

        // Additional properties for frontend display (not in entity)
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string SenderProfileImg { get; set; }
        public string ReceiverProfileImg { get; set; }
        public bool IsOwnMessage { get; set; } // Computed property for frontend

    }
}
