namespace BLL.Services.Abstractions
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
