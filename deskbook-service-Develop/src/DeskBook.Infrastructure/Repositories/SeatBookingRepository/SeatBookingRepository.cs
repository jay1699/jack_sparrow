using DeskBook.Infrastructure.Contracts.SeatBooking;
using DeskBook.Infrastructure.DeskbookDbContext;
using DeskBook.Infrastructure.Model.SeatRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeskBook.Infrastructure.Repositories.UserRegistrationRepository
{
    public class SeatBookingRepository : ISeatBookingRepository
    {
        private readonly DeskbookContext _context;

        private readonly ILogger<SeatBookingRepository> _logger;

        public SeatBookingRepository(DeskbookContext context, ILogger<SeatBookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<SeatBookingResponseModel>> GetBookingSeat(int pageNo, int pageSize, string? search, string? sort)
        {
            var result = from sr in _context.seatRequestModels
                         join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                         join s in _context.Seats on sr.SeatId equals s.SeatId
                         join c in _context.columns on s.ColumnId equals c.ColumnId
                         join f in _context.FloorModels on c.FloorId equals f.FloorId
                         where sr.RequestStatus == 2 && sr.DeletedDate == null
                         orderby sr.BookingDate descending
                         select new SeatBookingResponseModel
                         {
                             Name = e.FirstName + " " + e.LastName,
                             RequestDate = sr.CreatedDate,
                             AllottedDate = sr.BookingDate,
                             Email = e.EmailId,
                             FloorNo = f.FloorId,
                             DeskNo = $"{c.ColumnName}{s.SeatNumber}",
                         };
            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(r => r.Name.Contains(search));
            }
            if (sort == "Upcoming")
            {
                result = result.Where(x => x.AllottedDate >= DateTime.Today);
                return await result.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            if (sort == "Past Booking")
            {
                result = result.Where(x => x.AllottedDate < DateTime.Today);
                return await result.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            return await result.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}

