namespace BLL.Services.Impelementation
{
    public class FaceRecognitionService : IFaceRecognitionService
    {
        private readonly UserManager<User> _userManager;

        public FaceRecognitionService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<bool>> RegisterFaceAsync(Guid userId, string base64Image)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return Response<bool>.FailResponse("User not found");

                if (string.IsNullOrWhiteSpace(base64Image))
                    return Response<bool>.FailResponse("Invalid image data");

                // Validate base64 string
                if (!IsValidBase64(base64Image))
                    return Response<bool>.FailResponse("Invalid image format");

                // Store the base64 image
                user.SetFaceEncoding(base64Image);
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return Response<bool>.FailResponse("Failed to save face data");

                return Response<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.FailResponse($"Error registering face: {ex.Message}");
            }
        }

        public async Task<Response<string>> VerifyFaceAsync(string base64Image)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64Image))
                    return Response<string>.FailResponse("Invalid image data");

                // Face verification not implemented yet
                // This requires a face recognition library or cloud service
                return Response<string>.FailResponse("Face recognition verification is currently under development. Please use email/password login.");
            }
            catch (Exception ex)
            {
                return Response<string>.FailResponse($"Error verifying face: {ex.Message}");
            }
        }

        public async Task<Response<bool>> RemoveFaceDataAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return Response<bool>.FailResponse("User not found");

                user.RemoveFaceEncoding();
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return Response<bool>.FailResponse("Failed to remove face data");

                return Response<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.FailResponse($"Error removing face data: {ex.Message}");
            }
        }

        public async Task<Response<bool>> HasFaceDataAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return Response<bool>.FailResponse("User not found");

                return Response<bool>.SuccessResponse(user.HasFaceData);
            }
            catch (Exception ex)
            {
                return Response<bool>.FailResponse($"Error checking face data: {ex.Message}");
            }
        }

        private bool IsValidBase64(string base64String)
        {
            try
            {
                // Remove data URL prefix if present
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }

                // Try to convert to bytes
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}