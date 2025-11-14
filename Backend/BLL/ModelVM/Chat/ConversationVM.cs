

namespace BLL.ModelVM.Chat
{
    public class ConversationVM
    {
        public Guid OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string OtherUserProfileImg { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
    }
}
