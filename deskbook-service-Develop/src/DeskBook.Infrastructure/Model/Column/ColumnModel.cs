using System.Text.Json.Serialization;
using DeskBook.Infrastructure.Model.Floor;
using DeskBook.Infrastructure.Model.Seat;

namespace DeskBook.Infrastructure.Model.Column
{
    public class ColumnModel
    {
        public byte ColumnId { get; set; }

        public string ColumnName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public byte FloorId { get; set; }

        public FloorModel floorModel { get; set; }

        [JsonIgnore]
        public List<SeatModel> seats { get; set; }
    }
}