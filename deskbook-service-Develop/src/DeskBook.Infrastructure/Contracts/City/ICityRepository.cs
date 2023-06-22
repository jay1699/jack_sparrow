using DeskBook.Infrastructure.Model.City;

namespace DeskBook.Infrastructure.Contracts.City
{
    public interface ICityRepository
    {
        Task<List<CityModel>> GetCityDetail();
    }
}