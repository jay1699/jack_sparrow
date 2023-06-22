using DeskBook.AppServices.DTOs.City;
using DeskBook.AppServices.DTOs.Response;

namespace DeskBook.AppServices.Contracts.City
{

    public interface ICityServices
    {
        Task<ResponseDto<List<GetCityResultsDto>>> GetCityDetail();
    }
}
