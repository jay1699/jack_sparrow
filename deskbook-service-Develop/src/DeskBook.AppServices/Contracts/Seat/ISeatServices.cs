using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.Seat;

namespace DeskBook.AppServices.Contracts.Seat
{
    public interface ISeatServices
    {
        Task<ResponseDto<string>> AddSeat(List<AddSeatRequestDto> seatDto, string employeeId);

        Task<ResponseDto<string>> UpdateSeat(List<UpdateSeatRequestDto> seats, string employeeId);

        Task<ResponseDto<GetSeatResponseDto>> GetSeats(byte floorId);
    }
}