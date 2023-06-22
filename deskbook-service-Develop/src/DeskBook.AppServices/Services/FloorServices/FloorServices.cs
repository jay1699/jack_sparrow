using System.Net;
using DeskBook.AppServices.Contracts.Floors;
using DeskBook.AppServices.DTOs.Floors;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.Infrastructure.Contracts.FloorRepository;
using Microsoft.Extensions.Logging;

namespace DeskBook.AppServices.Services.FloorServices
{
    public class FloorServices : IFloorServices
    {
        private readonly ILogger<FloorServices> _logger;

        private readonly IFloorRepository _floorrepository;

        public FloorServices(IFloorRepository floorrepository, ILogger<FloorServices> logger)
        {
            _floorrepository = floorrepository;
            _logger = logger;
        }
        
        public async Task<ResponseDto<List<GetFloorResultDto>>> GetAllFloors(int cityId)
        {
            var data = await _floorrepository.GetAllFloors(cityId);
            var floors = new List<GetFloorResultDto>();
            foreach (var floor in data)
            {
                var floordto = new GetFloorResultDto();

                floordto.FloorId = floor.FloorId;
                floordto.FloorName = floor.FloorName;
                floors.Add(floordto);

            }
            var responseDto = new ResponseDto<List<GetFloorResultDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = floors
            };
            _logger.LogInformation("Retrieved Floors Successfully.");
            return responseDto;
        }
    }
}
