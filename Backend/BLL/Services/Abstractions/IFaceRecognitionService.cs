namespace BLL.Services.Abstractions
{
    public interface IFaceRecognitionService
    {

        Task<Response<bool>> RegisterFaceAsync(Guid userId, string base64Image);

        Task<Response<string>> VerifyFaceAsync(string base64Image);


        Task<Response<bool>> RemoveFaceDataAsync(Guid userId);

        Task<Response<bool>> HasFaceDataAsync(Guid userId);
    }
}