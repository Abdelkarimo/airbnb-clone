

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> Initiate([FromBody] CreatePaymentVM model)
        {
            var sub = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(sub)) return Unauthorized();
            if (!Guid.TryParse(sub, out var userId)) return Unauthorized();

            var resp = await _paymentService.InitiatePaymentAsync(userId, model.BookingId, model.Amount, model.PaymentMethod);
            if (!resp.Success) return BadRequest(resp.errorMessage);
            return Ok(resp.result);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] dynamic body)
        {
            int bookingId = (int)body.bookingId;
            string tx = (string)body.transactionId;
            var resp = await _paymentService.ConfirmPaymentAsync(bookingId, tx);
            if (!resp.Success) return BadRequest(resp.errorMessage);
            return Ok(resp.result);
        }

        [HttpPost("{id}/refund")]
        public async Task<IActionResult> Refund(int id)
        {
            var resp = await _paymentService.RefundPaymentAsync(id);
            if (!resp.Success) return BadRequest(resp.errorMessage);
            return Ok(resp.result);
        }
    }
}
