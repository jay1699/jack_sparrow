using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.SeatRequest;
using DeskBook.AppServices.Services.SeatRequest;

namespace DeskBook.AppServices.Contracts.SeatRequest
{
    public interface IUserSeatRequestServices
    {
        Task<ResponseDto<List<GetSeatRequestResultsDto>>> GetAllSeatRequest(int pageNo, int pageSize, string search, string sort);

        Task<ResponseDto<GetSeatRequestByIdResponseDto>> GetSeatRequestById(int seatRequestId);

        Task<ResponseDto<string>> UpdateSeatRequest(int seatRequestId, UpdateUserSeatRequestDto seatRequestDto, string UserRegisteredId);

        Task AutoApproval(CancellationToken cancellationToken);
    }
}