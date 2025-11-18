


namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Helper to read user id from multiple possible claim names
        private Guid? GetUserIdFromClaims()
        {
            var possible = new[]
            {
                 ClaimTypes.NameIdentifier,
                 "sub",
                 JwtRegisteredClaimNames.Sub,
                 "id",
                 "uid"
            };

            foreach (var name in possible)
            {
                var claim = User.FindFirst(name)?.Value;
                if (!string.IsNullOrEmpty(claim) && Guid.TryParse(claim, out var g))
                    return g;
            }

            // sometimes NameIdentifier is numeric or string id used by Identity; try Name
            var nameClaim = User.Identity?.Name;
            if (!string.IsNullOrEmpty(nameClaim) && Guid.TryParse(nameClaim, out var byName))
                return byName;

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingVM model)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var resp = await _bookingService.CreateBookingAsync(userId.Value, model);
            if (!resp.Success) return BadRequest(resp.errorMessage);
            return Ok(resp.result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var resp = await _bookingService.CancelBookingAsync(userId.Value, id);
            if (!resp.Success) return BadRequest(resp.errorMessage);
            return Ok(resp.result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> MyBookings()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var resp = await _bookingService.GetBookingsByUserAsync(userId.Value);
            if (!resp.Success) return BadRequest(resp.errorMessage);
            return Ok(resp.result);
        }

        [HttpGet("host/me")]
        public async Task<IActionResult> HostBookings()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var resp = await _bookingService.GetBookingsByHostAsync(userId.Value);
            if (!resp.Success) return BadRequest(resp.errorMessage);
            return Ok(resp.result);
        }
    }
}
