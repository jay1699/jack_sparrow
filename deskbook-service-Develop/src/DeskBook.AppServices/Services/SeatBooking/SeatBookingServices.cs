using System.Net;
using DeskBook.AppServices.Contracts.SeatBooking;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.SeatBooking;
using DeskBook.Infrastructure.Contracts.SeatBooking;
using DeskBook.Infrastructure.Resource;
using Microsoft.Extensions.Logging;

namespace DeskBook.AppServices.Services.SeatBooking
{
    public class SeatBookingServices : ISeatBookingServices
    {
        private readonly ISeatBookingRepository _seatBookingrepository;
        private readonly ILogger<SeatBookingServices> _logger;

        public SeatBookingServices(ISeatBookingRepository seatBookingrepository, ILogger<SeatBookingServices> logger)
        {
            _logger = logger;
            _seatBookingrepository = seatBookingrepository;

        }
        public async Task<ResponseDto<List<SeatBookingResponseDto>>> GetBookingSeat(int pageNo, int pageSize, string? search, string? sort)
        {
            if (search !=null && search.Length < 2)
            {
                _logger.LogError("Minnimum three character required");
                var badResponse = new ResponseDto<List<SeatBookingResponseDto>>()
                {
                    Data = null,
                    Error = new List<string> { ResponseMessage.MinCharacter },
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return badResponse;
            }
            var data = await _seatBookingrepository.GetBookingSeat(pageNo, pageSize, search, sort);
            if (data == null)
            {
                _logger.LogError("No data found");
                var dataresponse = new ResponseDto<List<SeatBookingResponseDto>>()
                {
                    Data = null,
                    Error = new List<string> { ResponseMessage.NoDataFound },
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return dataresponse;
            }
            var seats = new List<SeatBookingResponseDto>();
            foreach (var seat in data)
            {
                var seatResponse = new SeatBookingResponseDto
                {
                    Name = seat.Name,
                    Email = seat.Email,
                    RequestDate = seat.RequestDate.ToString("dd-MM-yyyy"),
                    AllottedDate = seat.AllottedDate.ToString("dd-MM-yyyy"),
                    FloorNo = seat.FloorNo,
                    DeskNo = seat.DeskNo
                };
                seats.Add(seatResponse);
            }
            _logger.LogError("Data retrieved successfully");
            var response = new ResponseDto<List<SeatBookingResponseDto>>()
            {
                Data = seats,
                StatusCode = (int)HttpStatusCode.OK
            };
            return response;
        }
    }
}