using BLL.ModelVM.Auth;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceRecognitionController : ControllerBase
    {
        private readonly IFaceRecognitionService _faceRecognitionService;
        private readonly IIdentityService _identityService;
        private readonly UserManager<User> _userManager;

        public FaceRecognitionController(
            IFaceRecognitionService faceRecognitionService,
            IIdentityService identityService,
            UserManager<User> userManager)
        {
            _faceRecognitionService = faceRecognitionService;
            _identityService = identityService;
            _userManager = userManager;
        }

        private Guid? GetUserIdFromClaims()
        {
            var sub = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(sub) && Guid.TryParse(sub, out var g))
                return g;
            return null;
        }

        /// <summary>
        /// Register face for authenticated user
        /// </summary>
        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> RegisterFace([FromBody] RegisterFaceVM model)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated" });

            var result = await _faceRecognitionService.RegisterFaceAsync(userId.Value, model.Base64Image);

            if (!result.Success)
                return BadRequest(new { message = result.errorMessage });

            return Ok(new { message = "Face registered successfully", hasFaceData = true });
        }

        /// <summary>
        /// Login using face recognition
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithFace([FromBody] FaceLoginVM model)
        {
            var result = await _identityService.LoginWithFaceAsync(model.Base64Image);

            if (!result.Success)
                return Unauthorized(new { message = result.errorMessage });

            // Extract user info from token to send back
            var userId = ExtractUserIdFromToken(result.result!);
            var user = await _userManager.FindByIdAsync(userId);

            return Ok(new FaceLoginResponseVM
            {
                Token = result.result!,
                UserId = user!.Id.ToString(),
                Email = user.Email!,
                FullName = user.FullName
            });
        }

        /// <summary>
        /// Remove face data for authenticated user
        /// </summary>
        [HttpDelete("remove")]
        [Authorize]
        public async Task<IActionResult> RemoveFaceData()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated" });

            var result = await _faceRecognitionService.RemoveFaceDataAsync(userId.Value);

            if (!result.Success)
                return BadRequest(new { message = result.errorMessage });

            return Ok(new { message = "Face data removed successfully", hasFaceData = false });
        }

        /// <summary>
        /// Check if user has face data registered
        /// </summary>
        [HttpGet("has-face-data")]
        [Authorize]
        public async Task<IActionResult> HasFaceData()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated" });

            var result = await _faceRecognitionService.HasFaceDataAsync(userId.Value);

            return Ok(new { hasFaceData = result.result });
        }

        /// <summary>
        /// Verify a face (returns user ID if found)
        /// </summary>
        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyFace([FromBody] FaceLoginVM model)
        {
            var result = await _faceRecognitionService.VerifyFaceAsync(model.Base64Image);

            if (!result.Success)
                return NotFound(new { message = result.errorMessage });

            var user = await _userManager.FindByIdAsync(result.result!);

            return Ok(new
            {
                userId = result.result,
                email = user?.Email,
                fullName = user?.FullName
            });
        }

        private string ExtractUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.First(c => c.Type == "sub").Value;
        }
    }
}