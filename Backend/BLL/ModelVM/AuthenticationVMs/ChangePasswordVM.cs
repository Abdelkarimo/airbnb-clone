namespace BLL.ModelVM.AuthenticationVMs
{
    public class ChangePasswordVM
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
