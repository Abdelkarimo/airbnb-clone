namespace BLL.ModelVM.AuthenticationVMs
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserRole Role { get; set; }
        public string? ProfileImg { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
