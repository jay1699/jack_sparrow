using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.SeatBooking;

namespace DeskBook.AppServices.Contracts.SeatBooking
{
    public interface ISeatBookingServices
    {
        Task<ResponseDto<List<SeatBookingResponseDto>>> GetBookingSeat(int pageNo, int pageSize, string? search, string? sort);
    }
}