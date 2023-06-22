using DeskBook.Infrastructure.Model.City;
using DeskBook.Infrastructure.Model.Column;
using DeskBook.Infrastructure.Model.Designation;
using DeskBook.Infrastructure.Model.Floor;
using DeskBook.Infrastructure.Model.Seat;
using DeskBook.Infrastructure.Model.SeatRequest;
using DeskBook.Infrastructure.Model.UserRegistration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DeskBook.Infrastructure.DeskbookDbContext
{
    public class DeskbookContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DeskbookContext(DbContextOptions<DeskbookContext> options) : base(options)
        {

        }

        public DbSet<SeatConfigurationModel> seatConfigurations { get; set; }

        public DbSet<SeatModel> Seats { get; set; }

        public DbSet<ColumnModel> columns { get; set; }

        public DbSet<FloorModel> FloorModels { get; set; }

        public DbSet<UserRegistrationModel> UserRegistrations { get; set; }

        public DbSet<EmployeeWorkingDaysModel> employeeWorkingDays { get; set; }

        public DbSet<CityModel> cityModels { get; set; }

        public DbSet<ModeOfWorkModel> modeOfWorks { get; set; }

        public DbSet<DesignationModel> designationModels { get; set; }

        public DbSet<WorkingDayModel> workingDayModels { get; set; }

        public DbSet<SeatRequestModel> seatRequestModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(DeskbookContext)));

            modelBuilder.Entity<UserRegistrationModel>()
                        .ToTable("Employee", "dbo")
                        .HasKey(x => x.EmployeeId);

            modelBuilder.Entity<SeatConfigurationModel>()
                        .ToTable("SeatConfiguration", "dbo")
                        .HasKey(x => x.SeatConfigurationId);

            modelBuilder.Entity<SeatConfigurationModel>()
                        .HasOne(x => x.userRegistrationModel)
                        .WithOne(x => x.seatConfigurationModel)
                        .HasForeignKey<SeatConfigurationModel>(x => x.EmployeeId);

            modelBuilder.Entity<DesignationModel>()
                        .ToTable("Designation", "Ref")
                        .HasKey(x => x.DesignationId);

            modelBuilder.Entity<ModeOfWorkModel>()
                        .ToTable("ModeOfWork", "Ref")
                        .HasKey(x => x.ModeOfWorkId);

            modelBuilder.Entity<WorkingDayModel>()
                        .ToTable("WorkingDay", "Ref")
                        .HasKey(x => x.WorkingDayId);

            modelBuilder.Entity<SeatConfigurationModel>()
                        .ToTable("SeatConfiguration", "dbo")
                        .HasKey(x => x.SeatConfigurationId);

            modelBuilder.Entity<CityModel>()
                        .ToTable("City", "Ref")
                        .HasKey(x => x.CityId);

            modelBuilder.Entity<FloorModel>()
                        .ToTable("Floor", "Ref")
                        .HasKey(x => x.FloorId);

            modelBuilder.Entity<ColumnModel>()
                        .ToTable("Column", "Ref")
                        .HasKey(x => x.ColumnId);

            modelBuilder.Entity<SeatModel>()
                        .ToTable("Seat", "Ref")
                        .HasKey(x => x.SeatId);

            modelBuilder.Entity<EmployeeWorkingDaysModel>()
                        .ToTable("EmployeeWorkingDays", "dbo")
                        .HasKey(x => x.EmployeeWorkingDayId);

            modelBuilder.Entity<SeatRequestModel>()
                        .ToTable("SeatRequest", "Booking")
                        .HasKey(x => x.SeatRequestId);


            modelBuilder.Entity<SeatModel>()
                        .HasOne(x => x.columnModel)
                        .WithMany(x => x.seats)
                        .HasForeignKey(x => x.ColumnId);

            modelBuilder.Entity<ColumnModel>()
                        .HasMany(x => x.seats)
                        .WithOne(x => x.columnModel)
                        .HasForeignKey(x => x.ColumnId);

            modelBuilder.Entity<SeatModel>()
                        .HasOne(x => x.user)
                        .WithMany(x => x.seats)
                        .HasForeignKey(x => x.CreatedBy);

            modelBuilder.Entity<ColumnModel>()
                        .HasOne(x => x.floorModel)
                        .WithMany(x => x.columnModels)
                        .HasForeignKey(x => x.FloorId);

            modelBuilder.Entity<FloorModel>()
                        .HasOne(x => x.cityModel)
                        .WithMany(x => x.floorModels)
                        .HasForeignKey(x => x.CityId);

            modelBuilder.Entity<SeatConfigurationModel>()
                        .HasOne(x => x.userRegistrationModel)
                        .WithOne(x => x.seatConfigurationModel)
                        .HasForeignKey<SeatConfigurationModel>(x => x.EmployeeId);

            modelBuilder.Entity<SeatConfigurationModel>()
                        .HasOne(x => x.seatModels)
                        .WithMany(x => x.seatConfigurationModel)
                        .HasForeignKey(x => x.SeatId);

            modelBuilder.Entity<EmployeeWorkingDaysModel>()
                        .HasOne(x => x.userRegistrationModel)
                        .WithOne(x => x.employeeWorkingDays)
                        .HasForeignKey<EmployeeWorkingDaysModel>(x => x.EmployeeId);

            modelBuilder.Entity<DesignationModel>()
                        .HasOne(x => x.userRegistrationModel)
                        .WithOne(x => x.designationModel)
                        .HasForeignKey<UserRegistrationModel>(x => x.DesignationId);

            modelBuilder.Entity<SeatRequestModel>()
                        .HasOne(x => x.user)
                        .WithMany(x => x.seatRequests)
                        .HasForeignKey(x => x.EmployeeId);

            modelBuilder.Entity<SeatRequestModel>()
                        .HasOne(x => x.seat)
                        .WithMany(x => x.seatRequests)
                        .HasForeignKey(x => x.SeatId);

            modelBuilder.Entity<SeatRequestModel>()
                        .HasOne(x => x.modifiedUser)
                        .WithMany()
                        .HasForeignKey(x => x.ModifiedBy);

        }
    }
}