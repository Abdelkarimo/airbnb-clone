namespace BLL.Services.Abstractions
{
    public interface IAuthenticationService
    {
        Task<AuthResponseVM> RegisterAsync(RegisterVM registerVM);
        Task<AuthResponseVM> LoginAsync(LoginVM loginVM);
        Task<AuthResponseVM> FirebaseLoginAsync(FirebaseLoginVM firebaseLoginVM);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordVM changePasswordVM);
    }
}
