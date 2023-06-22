using System.Net;
using DeskBook.AppServices.Services.CityServices;
using DeskBook.Infrastructure.Contracts.City;
using DeskBook.Infrastructure.Contracts.FloorRepository;
using DeskBook.Infrastructure.Contracts.Seat;
using DeskBook.Infrastructure.Model.City;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.City
{
    public class CityServicesTest
    {
        private readonly CitiesServices _citiesServices;
        private readonly Mock<ILogger<CitiesServices>> _logger;
        private readonly Mock<ICityRepository> _cityRepository;
        private readonly Mock<ISeatRepository> _seatRepository;
        private readonly Mock<IFloorRepository> _floorRepository;

        public CityServicesTest()
        {
            _logger = new Mock<ILogger<CitiesServices>>();
            _cityRepository = new Mock<ICityRepository>();
            _citiesServices = new CitiesServices(_cityRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task To_GetCities_ReturnsListOfCities()
        {
            //Arrange
            var city = new List<CityModel>
            {
                new CityModel { CityId = 1, CityName = "Surat" },
                new CityModel { CityId = 2, CityName = "Valsad" }
            };

            _cityRepository.Setup(r => r.GetCityDetail()).ReturnsAsync(city);

            // Act
            var result = await _citiesServices.GetCityDetail();

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);

            var firstCity = result.Data[0];
            Assert.Equal(1, firstCity.CityId);
            Assert.Equal("Surat", firstCity.CityName);
            var secondCity = result.Data[1];
            Assert.Equal(2, secondCity.CityId);
            Assert.Equal("Valsad", secondCity.CityName);
        }
    }
}