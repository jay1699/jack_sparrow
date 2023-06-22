using DeskBook.Infrastructure.Model.Seat;

namespace DeskBook.Infrastructure.Contracts.Seat
{
    public interface ISeatRepository
    {
        Task<int> GetAllSeatCount(byte columnId);

        Task<int> GetSeatNumberByColumnId(int id);

        Task AddSeat(List<SeatModel> seat);

        Task<SeatModel> GetSeatById(int seatid);

        Task UpdateSeat(List<SeatModel> seatModels);

        Task UpdateSeatConfiguration(List<SeatConfigurationModel> seatConfigurationModels);

        Task<SeatConfigurationModel> GetSeatConfigurationById(int seatid);

        Task<SeatConfigurationModel> GetBySeats(string employeeId);

        Task<List<SeatResponseModel>> GetSeat(byte floorId);

        Task UpdateSeatStatus(List<SeatConfigurationModel> seatConfiguration);

        Task<SeatModel> GetLastSeat(byte columnId);
    }
}


