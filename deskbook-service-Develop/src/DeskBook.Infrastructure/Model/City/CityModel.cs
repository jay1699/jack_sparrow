using DeskBook.Infrastructure.Model.Floor;

namespace DeskBook.Infrastructure.Model.City
{
    public class CityModel
    {
        public int CityId { get; set; }

        public string? CityName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public List<FloorModel> floorModels { get; set; }
    }
}