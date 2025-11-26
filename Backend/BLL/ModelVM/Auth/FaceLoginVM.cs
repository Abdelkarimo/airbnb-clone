namespace BLL.ModelVM.Auth
{
    public class FaceLoginVM
    {
        public string Base64Image { get; set; } = null!;
    }

    public class RegisterFaceVM
    {
        public string Base64Image { get; set; } = null!;
    }

    public class FaceLoginResponseVM
    {
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }
}