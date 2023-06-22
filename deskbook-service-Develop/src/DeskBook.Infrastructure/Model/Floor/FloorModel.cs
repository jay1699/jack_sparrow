using DeskBook.Infrastructure.Model.City;
using DeskBook.Infrastructure.Model.Column;

namespace DeskBook.Infrastructure.Model.Floor
{
    public class FloorModel
    {
        public byte FloorId { get; set; }

        public string FloorName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public int CityId { get; set; }

        public CityModel cityModel { get; set; }

        public List<ColumnModel> columnModels { get; set; }
    }
}