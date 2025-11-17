using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelVM.BookingVMs
{
    public class BookingDetailsVM
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public string ListingTitle { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public   BookingPaymentStatus PaymentStatus { get; set; }
    }
}
