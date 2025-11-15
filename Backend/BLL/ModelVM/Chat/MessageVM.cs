
namespace BLL.ModelVM.Chat
{
    public class MessageVM
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }  // Changed from Timestamp to SentAt
        public bool IsRead { get; set; }

        // Frontend display properties
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string SenderProfileImg { get; set; }
        public string ReceiverProfileImg { get; set; }
        public bool IsOwnMessage { get; set; }

    }
}
