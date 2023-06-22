using DeskBook.Infrastructure.Contracts.UserRegistration;
using DeskBook.Infrastructure.DeskbookDbContext;
using DeskBook.Infrastructure.Model.UserRegistration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeskBook.Infrastructure.Repositories.UserRegistrationRepository
{
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly DeskbookContext _context;

        private readonly ILogger<UserRegistrationRepository> _logger;

        public UserRegistrationRepository(DeskbookContext context, ILogger<UserRegistrationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddUser(UserRegistrationModel userRegistration)
        {
            _logger.LogInformation("User registration in Database is Successful");
            var result = await _context.UserRegistrations.AddAsync(userRegistration);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetByEmail(string email)
        {
            _logger.LogInformation("User GetbyEmail is successfull");
            var result = await _context.UserRegistrations.Where(x => x.EmailId == email)
                                                         .Select(x => x.EmailId)
                                                         .FirstOrDefaultAsync();
            return result;
        }

        public async Task<UserRegistrationModel> GetById(string employeeId)
        {
            var data = await _context.UserRegistrations.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.DeletedDate == null);
            return data;
        }

        public async Task UpdateUserStatus(List<UserRegistrationModel> userRegistrationModels)
        {
            _logger.LogInformation("The Account Has Been InActivated Successfully");
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetRegistredUserResponseModel>> GetUsers(int pageNo, int pageSize, string search)
        {
            IQueryable<GetRegistredUserResponseModel> query = from e in _context.UserRegistrations
                                                              join d in _context.designationModels on e.DesignationId equals d.DesignationId into des
                                                              from ed in des.DefaultIfEmpty()
                                                              select new GetRegistredUserResponseModel
                                                              {
                                                                  EmployeeId = e.EmployeeId,
                                                                  EmailId = e.EmailId,
                                                                  Name = e.FirstName + " " + e.LastName,
                                                                  DesignationName = ed.DesignationName,
                                                                  IsActive = e.IsActive,
                                                              };
            if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrEmpty(search))
            {
                query = query.Where(x => (x.Name.Contains(search)));
            }
            return await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

        }

        public async Task<UserRegistrationModel> GetEmployee(string employeeId)
        {
            var data = await _context.UserRegistrations.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.DeletedDate != null);
            return data;
        }

        public async Task<List<GetUserResponseModel>> GetEmployeeById(string employeeId)
        {
            var query = from e in _context.UserRegistrations
                        join sc in _context.seatConfigurations.Where(sc => sc.DeletedDate == null) on e.EmployeeId equals sc.EmployeeId into seatConfigs
                        from sc in seatConfigs.DefaultIfEmpty()
                        join s in _context.Seats on sc.SeatId equals s.SeatId into seats
                        from seat in seats.DefaultIfEmpty()
                        join cl in _context.columns on seat.ColumnId equals cl.ColumnId into columns
                        from column in columns.DefaultIfEmpty()
                        join f in _context.FloorModels on column.FloorId equals f.FloorId into floors
                        from floor in floors.DefaultIfEmpty()
                        join c in _context.cityModels on floor.CityId equals c.CityId into cities
                        from city in cities.DefaultIfEmpty()
                        join d in _context.designationModels on e.DesignationId equals d.DesignationId into designations
                        from designation in designations.DefaultIfEmpty()
                        join mow in _context.modeOfWorks on e.ModeOfWorkId equals mow.ModeOfWorkId into modesOfWork
                        from modeOfWork in modesOfWork.DefaultIfEmpty()
                        join ewd in _context.employeeWorkingDays on e.EmployeeId equals ewd.EmployeeId into employeeWorkingDays
                        from workingDay in employeeWorkingDays.Where(workingDay => workingDay.DeletedDate == null).DefaultIfEmpty()
                        join wd in _context.workingDayModels on workingDay.WorkingDayId equals wd.WorkingDayId into workingDays
                        from workingDayModel in workingDays.DefaultIfEmpty()
                        where e.EmployeeId == employeeId
                        select new GetUserResponseModel
                        {
                            EmployeeId = e.EmployeeId,
                            EmailId = e.EmailId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            PhoneNumber = e.PhoneNumber,
                            ModeOfWorkId = modeOfWork.ModeOfWorkId,
                            ModeOfWork = modeOfWork.ModeOfWork,
                            ProfilePictureFileName = e.ProfilePictureFileName,
                            ProfilePictureFilePath = e.ProfilePictureFilePath,
                            DesignationId = designation.DesignationId,
                            DesignationName = designation.DesignationName,
                            IsActive = e.IsActive,
                            CityId = city.CityId,
                            CityName = city.CityName,
                            FloorId = floor.FloorId,
                            FloorName = floor.FloorName,
                            ColumnId = column.ColumnId,
                            ColumnName = column.ColumnName,
                            SeatId = seat.SeatId,
                            SeatNumber = seat.SeatNumber,
                            WorkingDayId = workingDay.WorkingDayId,
                            WorkingDay = workingDayModel.Day
                        };
            return await query.ToListAsync();
        }
    }
}
