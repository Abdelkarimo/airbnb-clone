using BLL.ModelVM.AuthenticationVMs;
using BLL.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// Register a new user with email and password
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseVM>> Register(RegisterVM registerVM)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerVM);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// Login user with email and password
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseVM>> Login(LoginVM loginVM)
        {
            try
            {
                var result = await _authService.LoginAsync(loginVM);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// Login user using Firebase social authentication
        [HttpPost("firebase-login")]
        public async Task<ActionResult<AuthResponseVM>> FirebaseLogin(FirebaseLoginVM firebaseLoginVM)
        {
            try
            {
                var result = await _authService.FirebaseLoginAsync(firebaseLoginVM);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// Change user password (requires authentication)
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordVM changePasswordVM)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return Unauthorized();

            var result = await _authService.ChangePasswordAsync(userGuid, changePasswordVM);
            if (result)
                return Ok(new { message = "Password changed successfully" });
            else
                return BadRequest(new { message = "Failed to change password" });
        }
    }
}
