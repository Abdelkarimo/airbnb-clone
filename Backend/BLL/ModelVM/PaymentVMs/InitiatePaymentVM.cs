using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelVM.PaymentVMs
{
    public class InitiatePaymentVM
    {
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
