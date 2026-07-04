namespace WebApplication1.Repositories.Bookings
{
    public interface IBookingRepo
    {
        Task<IEnumerable<Booking>> GetIncomingBookingAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<bool> UpdateBookingStatusAsync(int bookingId,string status);
    }
}
