using DeskBook.Infrastructure.Contracts.Seat;
using DeskBook.Infrastructure.DeskbookDbContext;
using DeskBook.Infrastructure.Model.Seat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeskBook.Infrastructure.Repositories.Seat
{
    public class SeatRepository : ISeatRepository
    {
        private readonly DeskbookContext _context;
        private readonly ILogger<SeatRepository> _logger;

        public SeatRepository(DeskbookContext context, ILogger<SeatRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<SeatResponseModel>> GetSeat(byte floorId)
        {
            var dayOfWeekMapping = new Dictionary<string, int>
            {
                { "Monday", 1 },
                { "Tuesday", 2 },
                { "Wednesday", 3 },
                { "Thursday", 4 },
                { "Friday", 5 }
            };
            var today = DateTime.Now.DayOfWeek.ToString();
            int dayId;
            var value = dayOfWeekMapping.TryGetValue(today, out dayId);

            var query = from s in _context.Seats
                        join cl in _context.columns on s.ColumnId equals cl.ColumnId into seatColumns
                        from cl in seatColumns.DefaultIfEmpty()
                        join f in _context.FloorModels on cl.FloorId equals f.FloorId into columnFloors
                        from f in columnFloors.DefaultIfEmpty()
                        join c in _context.cityModels on f.CityId equals c.CityId into floorCities
                        from c in floorCities.DefaultIfEmpty()
                        join sc in _context.seatConfigurations.Where(sc => sc.DeletedDate == null) on s.SeatId equals sc.SeatId into seatConfigurations
                        from sc in seatConfigurations.DefaultIfEmpty()
                        join e in _context.UserRegistrations on sc.EmployeeId equals e.EmployeeId into seatEmployees
                        from e in seatEmployees.DefaultIfEmpty()
                        join mow in _context.modeOfWorks on e.ModeOfWorkId equals mow.ModeOfWorkId into employeeModesOfWork
                        from mow in employeeModesOfWork.DefaultIfEmpty()
                        join d in _context.designationModels on e.DesignationId equals d.DesignationId into employeeDesignations
                        from d in employeeDesignations.DefaultIfEmpty()
                        join ewd in _context.employeeWorkingDays.Where(ewd => ewd.WorkingDayId == dayId && ewd.DeletedDate == null) on sc.EmployeeId equals ewd.EmployeeId into employeeWorkingDays
                        from ewd in employeeWorkingDays.DefaultIfEmpty()
                        join wd in _context.workingDayModels on ewd.WorkingDayId equals wd.WorkingDayId into workingDayDetails
                        from wd in workingDayDetails.DefaultIfEmpty()
                        where f.FloorId == floorId
                        select new SeatResponseModel
                        {
                            SeatId = s.SeatId,
                            SeatNumber = s.SeatNumber,
                            CreatedDate = s.CreatedDate,
                            CreatedBy = s.CreatedBy,
                            ColumnId = cl.ColumnId,
                            ColumnName = cl.ColumnName,
                            IsAvailable = s.IsAvailable,
                            ColumnModel = cl,
                            SeatConfigurationModel = sc,
                            EmployeeId = sc.EmployeeId,
                            ModeOfWork = mow.ModeOfWork,
                            DesignationName = d.DesignationName,
                            Day = wd.Day,
                            SeatStatus = (s.IsAvailable == false ? "Yellow" :
                    (d.DesignationName == "ADMIN" ? "RED" :
                    (d.DesignationName == "INFRA" ? "Red" :
                    (d.DesignationName == "HR" ? "Red" :
                    (d.DesignationName == "DEVOPS" ? "Red" :
                    (d.DesignationName == "ACCOUNTS" ? "Red" :
                    (mow.ModeOfWork == "Regular" ? "Blue" :
                    (e.EmployeeId == null ? "Grey" :
                    (ewd.WorkingDayId == dayId ? "Blue" : "Green")))))))))
                        };
            return await query.ToListAsync();
        }

        public async Task<int> GetAllSeatCount(byte columnId)
        {
            var floorId = _context.columns.FirstOrDefault(x => x.ColumnId == columnId)?.FloorId;
            var seatCount = await (from c in _context.columns
                                   join s in _context.Seats on c.ColumnId equals s.ColumnId
                                   where c.FloorId == floorId
                                   select s).CountAsync();
            return seatCount;
        }

        public async Task<int> GetSeatNumberByColumnId(int id)
        {
            var result = await _context.Seats.Where(x => x.ColumnId == id).ToListAsync();
            return result.Count();
        }

        public async Task AddSeat(List<SeatModel> seat)
        {
            _logger.LogInformation("Seat Added Successfully");
            await _context.Seats.AddRangeAsync(seat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSeat(List<SeatModel> seatModels)
        {
            _logger.LogInformation("Seat Updated Successful");
            await _context.SaveChangesAsync();
        }

        public async Task<SeatModel> GetSeatById(int seatid)
        {
            _logger.LogInformation("Seat Updated Successful");
            var seat = await _context.Seats.FirstOrDefaultAsync(x => x.SeatId == seatid);
            return seat;
        }

        public async Task UpdateSeatConfiguration(List<SeatConfigurationModel> seatConfigurationModels)
        {
            _logger.LogInformation("Seat Unassigned Successful");
            await _context.SaveChangesAsync();
        }

        public async Task<SeatConfigurationModel> GetSeatConfigurationById(int seatid)
        {
            _logger.LogInformation("Seat Unassigned Successful");
            var seat = await _context.seatConfigurations.FirstOrDefaultAsync(x => x.SeatId == seatid && x.DeletedDate == null);
            return seat;
        }

        public async Task<SeatConfigurationModel> GetBySeats(string EmployeeId)
        {
            var data = await _context.seatConfigurations.FirstOrDefaultAsync(x => x.EmployeeId == EmployeeId && x.DeletedDate == null);
            return data;
        }

        public async Task UpdateSeatStatus(List<SeatConfigurationModel> seatConfiguration)
        {
            _logger.LogInformation("Seat Status Has Been Changed To Unassign");
            await _context.SaveChangesAsync();
        }

        public async Task<SeatModel> GetLastSeat(byte columnId)
        {
            var data = await _context.Seats.Where(x => x.ColumnId == columnId).OrderByDescending(s => s.SeatNumber).FirstOrDefaultAsync();
            return data;
        }
    }
}

