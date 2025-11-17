using BLL.ModelVM.BookingVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstractions
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(Guid guestId, CreateBookingVM model);
        Task<bool> CancelBookingAsync(int bookingId, Guid userId);
        Task<IEnumerable<Booking>> GetBookingsByGuestAsync(Guid userId);
        Task<IEnumerable<Booking>> GetBookingsByHostAsync(Guid hostId);


    }
}
