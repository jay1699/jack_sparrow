using System.Net;
using DeskBook.AppServices.Services.FloorServices;
using DeskBook.Infrastructure.Contracts.FloorRepository;
using DeskBook.Infrastructure.Model.Floor;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.Floors
{
    public class FloorServicesTest
    {
        private readonly FloorServices _floorServices;
        private readonly Mock<IFloorRepository> _floorRepository;
        private readonly Mock<ILogger<FloorServices>> _logger;

        public FloorServicesTest()
        {
            _logger = new Mock<ILogger<FloorServices>>();
            _floorRepository = new Mock<IFloorRepository>();
            _floorServices = new FloorServices(_floorRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task GetFloors_ReturnsOkResultWithFloorList()
        {
            // Arrange
            int cityId = 1;

            var floormodel = new List<FloorModel>
            {
                new FloorModel { FloorId = 1, FloorName = "First" },
                new FloorModel { FloorId = 2, FloorName = "Second" }
            };

            _floorRepository.Setup(x => x.GetAllFloors(cityId)).ReturnsAsync(floormodel);

            // Act
            var result = await _floorServices.GetAllFloors(cityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);

            Assert.Equal(1, result.Data[0].FloorId);
            Assert.Equal("First", result.Data[0].FloorName);
            Assert.Equal(2, result.Data[1].FloorId);
            Assert.Equal("Second", result.Data[1].FloorName);
        }
    }
}
