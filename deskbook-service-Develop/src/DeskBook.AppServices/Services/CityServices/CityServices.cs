using System.Net;
using DeskBook.AppServices.Contracts.City;
using DeskBook.AppServices.DTOs.City;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.Extension.ResponseExtention;
using DeskBook.Infrastructure.Contracts.City;
using DeskBook.Infrastructure.Resource;
using Microsoft.Extensions.Logging;

namespace DeskBook.AppServices.Services.CityServices
{
    public class CitiesServices : ICityServices
    {
        private readonly ICityRepository _cityRepository;

        private readonly ILogger<CitiesServices> _logger;

        public CitiesServices(ICityRepository cityRepository, ILogger<CitiesServices> logger)
        {
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task<ResponseDto<List<GetCityResultsDto>>> GetCityDetail()
        {
            var cities = new List<GetCityResultsDto>();
            var result = await _cityRepository.GetCityDetail();
            if (result != null)
            {
                _logger.LogError("Seat Added");
                foreach (var data in result)
                {
                    var citydto = new GetCityResultsDto();
                    citydto.CityId = data.CityId;
                    citydto.CityName = data.CityName;
                    cities.Add(citydto);
                }
                var successDto = new ResponseDto<List<GetCityResultsDto>>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = cities
                };
                _logger.LogInformation("Retrieved Cities Successfully.");
                return successDto;
            }
            _logger.LogError("Seat not Added");
            var responseDto = ResponseExtensions.ErrorResponse<List<GetCityResultsDto>>(null, ResponseMessage.InternalServerError, HttpStatusCode.InternalServerError);
            return responseDto;
        }
    }
}