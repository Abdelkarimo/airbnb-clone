

namespace BLL.ModelVM.Notification
{
    public class GetNotificationVM
    {
        public int Id { get; set; }

        //public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        
        // Action button for frontend navigation
        public string? ActionUrl { get; set; }
        public string? ActionLabel { get; set; }
    }
}
