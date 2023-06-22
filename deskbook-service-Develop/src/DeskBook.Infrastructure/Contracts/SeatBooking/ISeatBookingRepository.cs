using DeskBook.Infrastructure.Model.SeatRequest;

namespace DeskBook.Infrastructure.Contracts.SeatBooking
{
    public interface ISeatBookingRepository
    {
        Task<List<SeatBookingResponseModel>> GetBookingSeat(int pageNo, int pageSize, string? search, string? sort);
    }
}