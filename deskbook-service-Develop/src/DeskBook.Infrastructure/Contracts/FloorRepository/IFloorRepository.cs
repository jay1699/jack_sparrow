using DeskBook.Infrastructure.Model.Floor;

namespace DeskBook.Infrastructure.Contracts.FloorRepository
{
    public interface IFloorRepository
    {
        Task<List<FloorModel>> GetAllFloors(int cityId);
    }
}