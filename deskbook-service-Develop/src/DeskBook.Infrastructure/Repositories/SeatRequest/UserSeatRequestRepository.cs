using DeskBook.Infrastructure.Contracts.SeatRequest;
using DeskBook.Infrastructure.DeskbookDbContext;
using DeskBook.Infrastructure.Model.EmailModel;
using DeskBook.Infrastructure.Model.Enum;
using DeskBook.Infrastructure.Model.SeatRequest;
using DeskBook.Infrastructure.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace DeskBook.Infrastructure.Repositories.SeatRequest
{
    public class UserSeatRequestRepository : IUserSeatRequestRepository
    {
        private readonly DeskbookContext _context;
        private readonly ILogger<UserSeatRequestRepository> _logger;
        private readonly EmailModel _email;

        public UserSeatRequestRepository(DeskbookContext context, ILogger<UserSeatRequestRepository> logger, EmailModel email)
        {
            _context = context;
            _logger = logger;
            _email = email;
        }

        public async Task<List<GetSeatRequestResponseModel>> GetAllSeatRequest(int pageNo, int pageSize, string search, string sort)
        {
            IQueryable<GetSeatRequestResponseModel> query = from sr in _context.seatRequestModels
                                                            join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                                                            join s in _context.Seats on sr.SeatId equals s.SeatId
                                                            join c in _context.columns on s.ColumnId equals c.ColumnId
                                                            join f in _context.FloorModels on c.FloorId equals f.FloorId
                                                            where sr.DeletedDate == null
                                                            select new GetSeatRequestResponseModel
                                                            {
                                                                Name = e.FirstName + " " + e.LastName,
                                                                EmployeeId = e.EmployeeId,
                                                                SeatRequestId = sr.SeatRequestId,
                                                                RequestDate = sr.CreatedDate,
                                                                RequestFor = sr.BookingDate,
                                                                Email = e.EmailId,
                                                                FloorNo = f.FloorName,
                                                                DeskNo = $"{c.ColumnName}{s.SeatNumber}",
                                                                Status = sr.RequestStatus
                                                            };

            if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrEmpty(search))
            {
                query = query.Where(x => (x.Name.Contains(search)));
            }

            switch (sort)
            {
                case "All":
                    query = query.OrderByDescending(x => x.RequestDate);
                    break;
                case "Pending":
                    query = query.Where(x => x.Status == (int)RequestStatus.Pending).OrderByDescending(x => x.RequestDate);
                    break;
                case "Accepted":
                    query = query.Where(x => x.Status == (int)RequestStatus.Accepted).OrderByDescending(x => x.RequestDate);
                    break;
                case "Rejected":
                    query = query.Where(x => x.Status == (int)RequestStatus.Rejected).OrderByDescending(x => x.RequestDate);
                    break;
            }

            return await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<GetUserSeatRequestResponseModel>> GetMultipleSeatRequest(string employeeId, DateTime bookingdate, int seatRequestId)
        {
            var result = from sr in _context.seatRequestModels
                         join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                         join s in _context.Seats on sr.SeatId equals s.SeatId
                         join cl in _context.columns on s.ColumnId equals cl.ColumnId
                         join f in _context.FloorModels on cl.FloorId equals f.FloorId
                         join c in _context.cityModels on f.CityId equals c.CityId
                         where sr.EmployeeId == employeeId && sr.SeatRequestId != seatRequestId && sr.BookingDate == bookingdate && sr.DeletedDate == null
                         select new GetUserSeatRequestResponseModel
                         {
                             Name = $"{e.FirstName} {e.LastName}",
                             Email = e.EmailId,
                             Floor = f.FloorName,
                             RequestStatus = sr.RequestStatus,
                             BookingDate = sr.BookingDate,
                             SeatNumber = $"{cl.ColumnName}{s.SeatNumber}",
                             Location = c.CityName,
                             SeatRequestId = sr.SeatRequestId
                         };
            return await result.ToListAsync();
        }

        public async Task<GetSeatRequestByIdResponseModel> GetSeatRequestById(int seatRequestId)
        {
            var query = from sr in _context.seatRequestModels
                        join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                        join s in _context.Seats on sr.SeatId equals s.SeatId
                        join c in _context.columns on s.ColumnId equals c.ColumnId
                        join f in _context.FloorModels on c.FloorId equals f.FloorId
                        join ci in _context.cityModels on f.CityId equals ci.CityId
                        where sr.SeatRequestId == seatRequestId && sr.DeletedDate == null
                        select new GetSeatRequestByIdResponseModel
                        {
                            Name = $"{e.FirstName} {e.LastName}",
                            EmployeeId = e.EmployeeId,
                            Email = e.EmailId,
                            FloorNo = f.FloorName,
                            SeatId = s.SeatId,
                            RequestDate = sr.CreatedDate,
                            RequestedFor = sr.BookingDate,
                            DeskNo = $"{c.ColumnName}{s.SeatNumber}",
                            Reason = sr.Reason,
                            City = ci.CityName,
                            SeatNumber = s.SeatNumber.ToString() + c.ColumnName
                        };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<SeatRequestModel> GetUserSeatRequest(int seatRequestId)
        {
            var query = await _context.seatRequestModels.FirstOrDefaultAsync(x => x.SeatRequestId == seatRequestId && x.DeletedDate == null);
            return query;
        }

        public async Task UpdateSeatRequest(List<SeatRequestModel> seatRequestModels)
        {
            _logger.LogInformation("Seat Request Updated Successful");
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSeatRequestById(SeatRequestModel seatRequestModels)
        {
            _logger.LogInformation("Seat Request Updated Successful");
            await _context.SaveChangesAsync();
        }

        public async Task<List<SeatRequestModel>> GetPendingSeatRequests()
        {
            var pendingRequests = await _context.seatRequestModels
                .Where(r => r.RequestStatus == (int)RequestStatus.Pending && r.BookingDate.Date == DateTime.Now.Date.AddDays(1) && r.DeletedDate == null)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
            return pendingRequests;
        }

        public async Task<SeatRequestModel> GetFirstRequestedSeat(DateTime bookingDate)
        {
            var firstRequstedSeat = await _context.seatRequestModels
                                                .Where(r => r.RequestStatus == (int)RequestStatus.Pending && r.BookingDate == bookingDate && r.DeletedDate == null)
                                                .OrderBy(r => r.CreatedDate)
                                                .FirstOrDefaultAsync();
            return firstRequstedSeat;
        }

        public async Task<List<GetUserSeatRequestResponseModel>> GetDescendingSeatRequests(string employeeId, int seatRequestId, DateTime bookingDate, int seatId)
        {
            var result = from sr in _context.seatRequestModels
                         join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                         join s in _context.Seats on sr.SeatId equals s.SeatId
                         join cl in _context.columns on s.ColumnId equals cl.ColumnId
                         join f in _context.FloorModels on cl.FloorId equals f.FloorId
                         join c in _context.cityModels on f.CityId equals c.CityId
                         where sr.SeatRequestId != seatRequestId && sr.BookingDate.Date == bookingDate && sr.DeletedDate == null && (sr.SeatId == seatId || sr.EmployeeId == employeeId)
                         orderby sr.CreatedDate
                         select new GetUserSeatRequestResponseModel
                         {
                             Name = $"{e.FirstName} {e.LastName}",
                             Email = e.EmailId,
                             Floor = f.FloorName,
                             RequestStatus = sr.RequestStatus,
                             BookingDate = sr.BookingDate,
                             SeatNumber = $"{cl.ColumnName}{s.SeatNumber}",
                             Location = c.CityName,
                             SeatRequestId = sr.SeatRequestId
                         };

            var firstRequstedSeat = await _context.seatRequestModels
                                                .Where(r => r.RequestStatus == (int)RequestStatus.Pending && r.BookingDate == bookingDate && r.DeletedDate == null)
                                                .OrderBy(r => r.CreatedDate)
                                                .FirstOrDefaultAsync();
            var filteredResult = await result.Where(r => r.SeatRequestId != firstRequstedSeat.SeatRequestId).ToListAsync();

            return filteredResult;

        }

        public async Task<List<GetUserSeatRequestResponseModel>> GetSeatRequestByBookingDate(string employeeId, int seatId, DateTime bookingDate)
        {
            var result = from sr in _context.seatRequestModels
                         join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                         join s in _context.Seats on sr.SeatId equals s.SeatId
                         join cl in _context.columns on s.ColumnId equals cl.ColumnId
                         join f in _context.FloorModels on cl.FloorId equals f.FloorId
                         join c in _context.cityModels on f.CityId equals c.CityId
                         where sr.EmployeeId != employeeId && sr.SeatId == seatId && sr.BookingDate.Date == bookingDate && sr.DeletedDate == null
                         select new GetUserSeatRequestResponseModel
                         {
                             Name = $"{e.FirstName} {e.LastName}",
                             Email = e.EmailId,
                             Floor = f.FloorName,
                             RequestStatus = sr.RequestStatus,
                             BookingDate = sr.BookingDate,
                             SeatNumber = $"{cl.ColumnName}{s.SeatNumber}",
                             Location = c.CityName,
                             SeatRequestId = sr.SeatRequestId
                         };
            return await result.ToListAsync();
        }

        public async Task<List<GetUserSeatRequestResponseModel>> GetDisapprovedSeat(int seatId, int seatRequestId, DateTime bookingDate)
        {
            var result = from sr in _context.seatRequestModels
                         join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                         join s in _context.Seats on sr.SeatId equals s.SeatId
                         join cl in _context.columns on s.ColumnId equals cl.ColumnId
                         join f in _context.FloorModels on cl.FloorId equals f.FloorId
                         join c in _context.cityModels on f.CityId equals c.CityId
                         where sr.SeatRequestId != seatRequestId && sr.BookingDate.Date == bookingDate && sr.SeatId == seatId && sr.DeletedDate == null
                         select new GetUserSeatRequestResponseModel
                         {
                             Name = $"{e.FirstName} {e.LastName}",
                             Email = e.EmailId,
                             Floor = f.FloorName,
                             RequestStatus = sr.RequestStatus,
                             BookingDate = sr.BookingDate,
                             SeatNumber = $"{cl.ColumnName}{s.SeatNumber}",
                             Location = c.CityName,
                             SeatRequestId = sr.SeatRequestId
                         };
            return await result.ToListAsync();
        }

        public async Task ApproveEmail(EmailResponseModel email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("From 1Rivet", _email.From));
            message.To.Add(new MailboxAddress("To Employee", email.To));
            message.Subject = EmailDetail.ApprovedSubject;

            string htmlTemplate = @"
        <html>
        <body>
            <p>Dear {5},</p>
            <br>
            <p>{6}</p>
            <p>Date: {0}</p>
            <p>Location: {1}</p>
            <p>Floor: {2}</p>
            <p>Seat Number: {3}</p>
            <p>Duration: {4}</p>
            <br>
            <p>Congratulations on the approval of your office seat.</p>
            <br>
            <p>Thanks,</p>
            <p><img src='cid:logo' alt='Logo'> | DeskBook</p>
        </body>
        </html>";

            var bodyBuilder = new BodyBuilder();
            var logoImagePath = _email.LogoLocation;

            var logoAttachment = new MimePart("image", "png")
            {
                ContentId = "logo",
                ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                ContentTransferEncoding = ContentEncoding.Base64
            };

            using (var stream = File.OpenRead(logoImagePath))
            {
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageData);
                logoAttachment.Content = new MimeContent(new MemoryStream(imageData));
                logoAttachment.ContentTransferEncoding = ContentEncoding.Base64;
            }


            bodyBuilder.LinkedResources.Add(logoAttachment);
            htmlTemplate = string.Format(htmlTemplate, email.BookingDate, email.OfficeLocation, email.FloorNumber, email.SeatNumber, email.Duration, email.EmployeeName, EmailDetail.ApprovedBody);
            bodyBuilder.HtmlBody = htmlTemplate;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.outlook.com", 587, false);
                client.Authenticate(_email.From, _email.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }

        public async Task RejectedMail(EmailResponseModel email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("From 1Rivet", _email.From));
            message.To.Add(new MailboxAddress("To Employee", email.To));
            message.Subject = EmailDetail.RejectedSubject;

            string htmlTemplate = @"
        <html>
        <body>
            <p>Dear {0},</p>
            <br>
            <p>{1}</p>
            <br>
            <p>Thanks,</p>
            <p><img src='cid:logo' alt='Deskbook Logo'> | Deskbook</p>
        </body>
        </html>";

            var bodyBuilder = new BodyBuilder();
            var logoImagePath = _email.LogoLocation;

            var logoAttachment = new MimePart("image", "png")
            {
                ContentId = "logo",
                ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                ContentTransferEncoding = ContentEncoding.Base64
            };

            using (var stream = File.OpenRead(logoImagePath))
            {
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageData);
                logoAttachment.Content = new MimeContent(new MemoryStream(imageData));
                logoAttachment.ContentTransferEncoding = ContentEncoding.Base64;
            }

            bodyBuilder.LinkedResources.Add(logoAttachment);
            htmlTemplate = string.Format(htmlTemplate, email.EmployeeName, EmailDetail.RejectedBody);
            bodyBuilder.HtmlBody = htmlTemplate;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.outlook.com", 587, false);
                client.Authenticate(_email.From, _email.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }

        }

        public async Task SendRejectedMailWithUpdate(List<GetUserSeatRequestResponseModel> rejectlist)
        {
            var seats = new List<SeatRequestModel>();
            if (rejectlist.Count > 0)
            {
                foreach (var reject in rejectlist)
                {
                    var rejected = await GetUserSeatRequest(reject.SeatRequestId);
                    rejected.RequestStatus = (int)RequestStatus.Rejected;
                    rejected.ModifiedBy = null;
                    rejected.ModifiedDate = DateTime.Now;
                    seats.Add(rejected);
                    var rejectedMail = new EmailResponseModel
                    {
                        EmployeeName = reject.Name,
                        Subject = EmailDetail.RejectedSubject,
                        Body = EmailDetail.RejectedBody,
                        To = reject.Email
                    };
                    await RejectedMail(rejectedMail);
                }
                await UpdateSeatRequest(seats);
            }


        }

        public async Task SendApprovedMail(GetUserSeatRequestResponseModel employee)
        {
            var email = new EmailResponseModel
            {
                EmployeeName = employee.Name,
                OfficeLocation = employee.Location,
                FloorNumber = employee.Floor,
                SeatNumber = employee.SeatNumber,
                BookingDate = employee.BookingDate.ToString(),
                Duration = "All Day",
                To = employee.Email
            };
            await ApproveEmail(email);
        }

        public async Task<List<GetUserSeatRequestResponseModel>> GetApprovedSeatRequest(int seatRequestId)
        {
            var result = from sr in _context.seatRequestModels
                         join e in _context.UserRegistrations on sr.EmployeeId equals e.EmployeeId
                         join s in _context.Seats on sr.SeatId equals s.SeatId
                         join cl in _context.columns on s.ColumnId equals cl.ColumnId
                         join f in _context.FloorModels on cl.FloorId equals f.FloorId
                         join c in _context.cityModels on f.CityId equals c.CityId
                         where sr.SeatRequestId == seatRequestId
                         orderby sr.CreatedDate
                         select new GetUserSeatRequestResponseModel
                         {
                             Name = $"{e.FirstName} {e.LastName}",
                             Email = e.EmailId,
                             Floor = f.FloorName,
                             RequestStatus = sr.RequestStatus,
                             BookingDate = sr.BookingDate,
                             SeatNumber = $"{cl.ColumnName}{s.SeatNumber}",
                             Location = c.CityName,
                             SeatRequestId = sr.SeatRequestId
                         };
            return await result.ToListAsync();
        }

    }
}