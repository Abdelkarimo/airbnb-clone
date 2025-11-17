using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstractions
{
    public interface IPaymentService
    {
       Task<string> InitiatePaymentAsync(int bookingId, string method);
       Task<bool> ConfirmPaymentAsync(string transactionId, string gatewayResponse);

    }
}
