

namespace BLL.ModelVM.Chat
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImg { get; set; }
        public DateTime DateCreated { get; set; }
        public string? FirebaseUid { get; set; }
        public bool IsActive { get; set; }
    }
}
