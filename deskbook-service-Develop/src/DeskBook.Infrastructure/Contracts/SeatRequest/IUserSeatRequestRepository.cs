using DeskBook.Infrastructure.Model.EmailModel;
using DeskBook.Infrastructure.Model.SeatRequest;

namespace DeskBook.Infrastructure.Contracts.SeatRequest
{
    public interface IUserSeatRequestRepository
    {
        Task<List<GetSeatRequestResponseModel>> GetAllSeatRequest(int pageNo, int pageSize, string search, string sort);

        Task<GetSeatRequestByIdResponseModel> GetSeatRequestById(int seatRequestId);

        Task<List<GetUserSeatRequestResponseModel>> GetDisapprovedSeat(int seatId, int seatRequestId, DateTime bookingDate);

        Task<SeatRequestModel> GetUserSeatRequest(int seatRequestId);

        Task<List<GetUserSeatRequestResponseModel>> GetMultipleSeatRequest(string employeeId, DateTime bookingdate, int seatRequestId);

        Task UpdateSeatRequest(List<SeatRequestModel> seatRequestModels);

        Task UpdateSeatRequestById(SeatRequestModel seatRequestModels);

        Task<List<SeatRequestModel>> GetPendingSeatRequests();

        Task<List<GetUserSeatRequestResponseModel>> GetDescendingSeatRequests(string employeeId, int seatRequestId, DateTime bookingDate,int seatId);

        Task<List<GetUserSeatRequestResponseModel>> GetApprovedSeatRequest(int seatRequestId);

        Task<SeatRequestModel> GetFirstRequestedSeat(DateTime bookingDate);

        Task<List<GetUserSeatRequestResponseModel>> GetSeatRequestByBookingDate(string employeeId, int seatId, DateTime bookingDate);

        Task RejectedMail(EmailResponseModel email);

        Task ApproveEmail(EmailResponseModel email);

        Task SendRejectedMailWithUpdate(List<GetUserSeatRequestResponseModel> rejectlist);

        Task SendApprovedMail(GetUserSeatRequestResponseModel employee);
        
    }
}

