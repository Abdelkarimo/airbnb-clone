namespace BLL.ModelVM.AuthenticationVMs
{
    public class AuthResponseVM
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public UserVM User { get; set; } = null!;
    }
}
