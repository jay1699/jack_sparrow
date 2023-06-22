using System.Net;
using DeskBook.AppServices.Contracts.SeatRequest;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.SeatRequest;
using DeskBook.Infrastructure.Contracts.SeatRequest;
using DeskBook.Infrastructure.Model.EmailModel;
using DeskBook.Infrastructure.Model.Enum;
using DeskBook.Infrastructure.Model.SeatRequest;
using DeskBook.Infrastructure.Resource;
using Microsoft.Extensions.Logging;

namespace DeskBook.AppServices.Services.SeatRequest
{
    public class UserSeatRequestServices : IUserSeatRequestServices
    {
        private readonly IUserSeatRequestRepository _userSeatRequestRepository;

        private readonly ILogger<UserSeatRequestServices> _logger;


        public UserSeatRequestServices(IUserSeatRequestRepository userSeatRequestRepository, ILogger<UserSeatRequestServices> logger)
        {
            _userSeatRequestRepository = userSeatRequestRepository;
            _logger = logger;
        }

        public async Task<ResponseDto<List<GetSeatRequestResultsDto>>> GetAllSeatRequest(int pageNo, int pageSize, string search, string sort)
        {

            if (search != null && search.Length < 2)
            {
                var response = new ResponseDto<List<GetSeatRequestResultsDto>>()
                {
                    Data = null,
                    Error = new List<string> { ResponseMessage.MinCharacter },
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return response;
            }
            var request = await _userSeatRequestRepository.GetAllSeatRequest(pageNo, pageSize, search, sort);

            var seatrequests = new List<GetSeatRequestResultsDto>();

            foreach (var result in request)
            {
                var seatrequest = new GetSeatRequestResultsDto();
                seatrequest.Name = result.Name;
                seatrequest.EmployeeId = result.EmployeeId;
                seatrequest.SeatRequestId = result.SeatRequestId;
                seatrequest.RequestDate = result.RequestDate.Date.ToString("dd-MM-yyyy");
                seatrequest.RequestFor = result.RequestFor.Date.ToString("dd-MM-yyyy");
                seatrequest.Email = result.Email;
                seatrequest.FloorNo = result.FloorNo;
                seatrequest.DeskNo = result.DeskNo;
                seatrequest.Status = result.Status;
                seatrequests.Add(seatrequest);
            }

            var responseDto = new ResponseDto<List<GetSeatRequestResultsDto>>()
            {
                Data = seatrequests,
                Error = null,
                StatusCode = (int)HttpStatusCode.OK
            };
            _logger.LogInformation("All Seat Request are retrieved successfully.");
            return responseDto;
        }


        public async Task<ResponseDto<GetSeatRequestByIdResponseDto>> GetSeatRequestById(int seatRequestId)
        {
            var getSeatRequest = await _userSeatRequestRepository.GetSeatRequestById(seatRequestId);
            if (getSeatRequest == null)
            {
                var badResponse = new ResponseDto<GetSeatRequestByIdResponseDto>()
                {
                    Data = null,
                    Error = new List<string> { ResponseMessage.RequestIdnotFound },
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return badResponse;
            }

            var seatRequestByIdResponseDto = new GetSeatRequestByIdResponseDto
            {
                Name = getSeatRequest.Name,
                EmployeeId = getSeatRequest.EmployeeId,
                Email = getSeatRequest.Email,
                FloorNo = getSeatRequest.FloorNo,
                SeatId = getSeatRequest.SeatId,
                RequestDate = getSeatRequest.RequestDate.Date.ToString("dd-MM-yyyy"),
                RequestedFor = getSeatRequest.RequestedFor.Date.ToString("dd-MM-yyyy"),
                DeskNo = getSeatRequest.DeskNo,
                Reason = getSeatRequest.Reason
            };

            var response = new ResponseDto<GetSeatRequestByIdResponseDto>()
            {
                Data = seatRequestByIdResponseDto,
                Error = null,
                StatusCode = 200
            };

            _logger.LogInformation("Employee Deatils Retrieved Sucessfully");
            return response;
        }


        public async Task<ResponseDto<string>> UpdateSeatRequest(int seatRequestId, UpdateUserSeatRequestDto seatRequestDto, string UserRegisteredId)
        {
            var result = await _userSeatRequestRepository.GetUserSeatRequest(seatRequestId);
            var rejectlist = await _userSeatRequestRepository.GetMultipleSeatRequest(seatRequestDto.EmployeeId, result.BookingDate, seatRequestId);
            if (result != null && seatRequestDto.RequestStatus == (int)RequestStatus.Accepted)
            {
                result.RequestStatus = seatRequestDto.RequestStatus;
                result.ModifiedBy = UserRegisteredId;
                result.ModifiedDate = DateTime.Now;
                await _userSeatRequestRepository.UpdateSeatRequestById(result);
                var seats = new List<SeatRequestModel>();
                if (rejectlist.Count > 0)
                {
                    foreach (var reject in rejectlist)
                    {
                        var rejected = await _userSeatRequestRepository.GetUserSeatRequest(reject.SeatRequestId);
                        rejected.RequestStatus = (int)RequestStatus.Rejected;
                        rejected.ModifiedBy = UserRegisteredId;
                        rejected.ModifiedDate = DateTime.Now;
                        seats.Add(rejected);
                    }
                    await _userSeatRequestRepository.UpdateSeatRequest(seats);
                }
                var employee = await _userSeatRequestRepository.GetSeatRequestById(result.SeatRequestId);
                var email = new EmailResponseModel
                {
                    EmployeeName = employee.Name,
                    Subject = EmailDetail.ApprovedSubject,
                    Body = EmailDetail.ApprovedBody,
                    BookingDate = employee.RequestDate.ToString(),
                    OfficeLocation = employee.City,
                    FloorNumber = employee.FloorNo,
                    SeatNumber = employee.SeatNumber,
                    Duration = "All Day",
                    To = employee.Email
                };
                await _userSeatRequestRepository.ApproveEmail(email);

                var disapprovedSeats = await _userSeatRequestRepository.GetDisapprovedSeat(result.SeatId, result.SeatRequestId, result.BookingDate);
                if (disapprovedSeats.Count > 0)
                {
                    foreach (var disapprovedSeat in disapprovedSeats)
                    {
                        var seatModel = new SeatRequestModel();
                        var rejected = await _userSeatRequestRepository.GetSeatRequestById(disapprovedSeat.SeatRequestId);
                        var disapproved = await _userSeatRequestRepository.GetUserSeatRequest(disapprovedSeat.SeatRequestId);
                        disapproved.RequestStatus = (int)RequestStatus.Rejected;
                        disapproved.ModifiedBy = UserRegisteredId;
                        disapproved.ModifiedDate = DateTime.Now;
                        await _userSeatRequestRepository.UpdateSeatRequestById(seatModel);
                        var rejectedemail = new EmailResponseModel
                        {
                            EmployeeName = rejected.Name,
                            Subject = EmailDetail.RejectedSubject,
                            Body = EmailDetail.RejectedBody,
                            To = rejected.Email
                        };
                        await _userSeatRequestRepository.RejectedMail(rejectedemail);
                    }
                }

            }
            if (result != null && seatRequestDto.RequestStatus == (int)RequestStatus.Rejected)
            {
                var rejected = await _userSeatRequestRepository.GetSeatRequestById(result.SeatRequestId);
                result.RequestStatus = seatRequestDto.RequestStatus;
                result.ModifiedBy = UserRegisteredId;
                result.ModifiedDate = DateTime.Now;
                await _userSeatRequestRepository.UpdateSeatRequestById(result);
                var email = new EmailResponseModel
                {
                    EmployeeName = rejected.Name,
                    Subject = EmailDetail.RejectedSubject,
                    Body = EmailDetail.RejectedBody,
                    To = rejected.Email
                };
                await _userSeatRequestRepository.RejectedMail(email);
            }
            var response = new ResponseDto<string>()
            {
                Data = ResponseMessage.SuccessfulChanges,
                Error = null,
                StatusCode = 200
            };
            return response;
        }

        public async Task AutoApproval(CancellationToken cancellationToken)
        {
            var delay = (DateTime.Today.AddDays(1) - DateTime.Now).Hours;
            await Task.Delay(TimeSpan.FromHours(delay + 1), cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                var pendingRequests = await _userSeatRequestRepository.GetPendingSeatRequests();
                foreach (var request in pendingRequests)
                {
                    var firstRequestedSeat = await _userSeatRequestRepository.GetFirstRequestedSeat(request.BookingDate);
                    var rejectlist = await _userSeatRequestRepository.GetDescendingSeatRequests(firstRequestedSeat.EmployeeId, firstRequestedSeat.SeatId, firstRequestedSeat.BookingDate,firstRequestedSeat.SeatId);
                    var dateUntilBooking = request.BookingDate.Date - DateTime.Now.Date;
                    if (dateUntilBooking.TotalDays <= 1)
                    {
                        firstRequestedSeat.RequestStatus = (int)RequestStatus.Accepted;
                        firstRequestedSeat.ModifiedBy = null;
                        firstRequestedSeat.ModifiedDate = DateTime.Now;
                        await _userSeatRequestRepository.UpdateSeatRequestById(firstRequestedSeat);
                        var seats = new List<SeatRequestModel>();
                        if (rejectlist.Count > 0)
                        {
                            await _userSeatRequestRepository.SendRejectedMailWithUpdate(rejectlist);
                        }
                    }
                    var employees = await _userSeatRequestRepository.GetApprovedSeatRequest(request.SeatRequestId);
                    var employee = employees.FirstOrDefault();
                    await _userSeatRequestRepository.SendApprovedMail(employee);
                }
                pendingRequests = await _userSeatRequestRepository.GetPendingSeatRequests();
            }
        }
    }
}

