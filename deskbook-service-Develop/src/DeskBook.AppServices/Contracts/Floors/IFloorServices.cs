using DeskBook.AppServices.DTOs.Floors;
using DeskBook.AppServices.DTOs.Response;

namespace DeskBook.AppServices.Contracts.Floors
{
    public interface IFloorServices
    {
        Task<ResponseDto<List<GetFloorResultDto>>> GetAllFloors(int cityId);
    }
}