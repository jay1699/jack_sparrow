using System.Net;
using DeskBook.AppServices.Contracts.Seat;
using Microsoft.Extensions.Logging;
using DeskBook.AppServices.DTOs.Seat;
using DeskBook.AppServices.Extension.ResponseExtention;
using DeskBook.Infrastructure.Model.Seat;
using DeskBook.Infrastructure.Contracts.Seat;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.Infrastructure.Resource;

namespace DeskBook.AppServices.Services.Seat
{
    public class SeatServices : ISeatServices
    {
        private readonly ISeatRepository _seatrepository;
        private readonly ILogger<SeatServices> _logger;

        public SeatServices(ISeatRepository seatRepository, ILogger<SeatServices> logger)
        {
            _logger = logger;
            _seatrepository = seatRepository;
        }

        public async Task<ResponseDto<GetSeatResponseDto>> GetSeats(byte floorId)
        {
            var result = await _seatrepository.GetSeat(floorId);
            var seatResponse = new GetSeatResponseDto
            {
                bookedSeat = new List<GetSeatResultsDto>(),
                availableforBookingSeat = new List<GetSeatResultsDto>(),
                reservedSeat = new List<GetSeatResultsDto>(),
                unavailableSeat = new List<GetSeatResultsDto>(),
                unallocatedSeat = new List<GetSeatResultsDto>()
            };

            foreach (var seat in result)
            {
                var getSeatDto = new GetSeatResultsDto
                {
                    ColumnName = seat.ColumnName,
                    SeatNumber = seat.SeatNumber,
                    SeatId = seat.SeatId,
                    ColumnId = seat.ColumnId
                };

                if (seat.SeatStatus == "Grey")
                {
                    seatResponse.unallocatedSeat.Add(getSeatDto);
                }
                else if (seat.SeatStatus == "Red" || seat.SeatStatus == "RED")
                {
                    seatResponse.reservedSeat.Add(getSeatDto);
                }
                else if (seat.SeatStatus == "Green")
                {
                    seatResponse.availableforBookingSeat.Add(getSeatDto);
                }
                else if (seat.SeatStatus == "Blue")
                {
                    seatResponse.bookedSeat.Add(getSeatDto);
                }
                else if (seat.SeatStatus == "Yellow")
                {
                    seatResponse.unavailableSeat.Add(getSeatDto);
                }
            }

            var response = new ResponseDto<GetSeatResponseDto>()
            {
                Data = seatResponse,
                StatusCode = (int)HttpStatusCode.OK
            };
            return response;
        }

        public async Task<ResponseDto<string>> AddSeat(List<AddSeatRequestDto> seatDto, string employeeId)
        {
            var seatModel = new List<SeatModel>();
            var lastSeat = new SeatModel();
            int columnCount = 0;
            int lastSeatNumber = 0;
            var groupedSeats = seatDto.GroupBy(s => s.ColumnId);

            foreach (var group in groupedSeats)
            {
                var columnId = group.Key;
                var seatsInColumn = group.ToList();

                foreach (var seat in seatsInColumn)
                {
                    var seatCount = await _seatrepository.GetAllSeatCount(columnId);

                    if (columnCount == 0)
                    {
                        columnCount = await _seatrepository.GetSeatNumberByColumnId(columnId);
                    }

                    if (lastSeatNumber == 0)
                    {
                        lastSeat = await _seatrepository.GetLastSeat(columnId);
                        lastSeatNumber = lastSeat.SeatNumber;
                    }

                    if (seatCount >= 150 || columnCount >= 15)
                    {
                        _logger.LogInformation("Maximum limit to add seat on one Table is 15 and capacity of one floor is 150 seats.");
                        var responseDto = ResponseExtensions.ErrorResponse<string>(null, ResponseMessage.MaximumLimit, HttpStatusCode.BadRequest);
                        return responseDto;
                    }
                    if (lastSeatNumber + 1 == seat.SeatNumber)
                    {
                        var newSeat = new SeatModel()
                        {
                            SeatNumber = seat.SeatNumber,
                            ColumnId = seat.ColumnId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = employeeId,
                            IsAvailable = true
                        };

                        lastSeatNumber++;
                        columnCount++;
                        seatModel.Add(newSeat);
                    }
                    else
                    {
                        _logger.LogInformation("Your SeatNumber is not in Consecutive Order.");
                        var response = ResponseExtensions.ErrorResponse<string>(null, ResponseMessage.ConsecutiveOrder, HttpStatusCode.BadRequest);
                        return response;
                    }
                }
                await _seatrepository.AddSeat(seatModel);
                seatModel = new List<SeatModel>();
                lastSeatNumber = 0;
                columnCount = 0;
            }

            var successResponse = new ResponseDto<string>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = ResponseMessage.SeatAdded
            };

            return successResponse;
        }

        public async Task<ResponseDto<string>> UpdateSeat(List<UpdateSeatRequestDto> seats, string employeeId)
        {
            var seatsModel = new List<SeatModel>();
            var seatconfig = new List<SeatConfigurationModel>();
            var isAvailable = false;
            var unassigned = false;
            foreach (var seat in seats)
            {
                var seatModel = await _seatrepository.GetSeatById(seat.SeatId);
                var seatConfiguration = await _seatrepository.GetSeatConfigurationById(seat.SeatId);
                if (!seat.IsAvailable && !seat.Unassigned)
                {
                    if (!seat.IsAvailable)
                    {
                        seatModel.IsAvailable = seat.IsAvailable;
                        seatModel.ModifiedDate = DateTime.Now;
                        seatModel.ModifiedBy = employeeId;
                        isAvailable = true;
                        seatsModel.Add(seatModel);
                    }
                }
                else if (seat.IsAvailable && seat.Unassigned)
                {
                    if (seat.Unassigned)
                    {
                        seatModel.IsAvailable = seat.IsAvailable;
                        seatModel.ModifiedDate = DateTime.Now;
                        seatModel.ModifiedBy = employeeId;
                        if (seatConfiguration != null)
                        {
                            seatConfiguration.DeletedDate = DateTime.Now;
                            seatConfiguration.DeletedBy = employeeId;
                            seatconfig.Add(seatConfiguration);
                        }
                        unassigned = true;
                        seatsModel.Add(seatModel);
                    }
                }
                else
                {
                    _logger.LogInformation("Seat status can not be changed");
                    var responseDto = ResponseExtensions.ErrorResponse<string>(null, ResponseMessage.StatusNotChanged, HttpStatusCode.BadRequest);
                    return responseDto;
                }
            }
            if (unassigned)
            {
                await _seatrepository.UpdateSeat(seatsModel);
                _logger.LogInformation("Seat status has been changed to Unassigned");
                var responseDto = new ResponseDto<string>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = ResponseMessage.UnassignStatus
                };
                return responseDto;
            }

            if (isAvailable)
            {
                await _seatrepository.UpdateSeatConfiguration(seatconfig);
                _logger.LogInformation("Seat status has been changed to Unavailable");
                var responseDto = new ResponseDto<string>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = ResponseMessage.UnavailableStatus
                };
                return responseDto;
            }

            var response = new ResponseDto<string>()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Data = ResponseMessage.ErrorOccured
            };
            return response;
        }
    }
}


