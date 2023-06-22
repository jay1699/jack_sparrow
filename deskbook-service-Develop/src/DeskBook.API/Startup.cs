using Deskbook.API.Config;
using DeskBook.AppServices.Contracts.City;
using DeskBook.AppServices.Contracts.Floors;
using DeskBook.AppServices.Extension;
using DeskBook.AppServices.Services.CityServices;
using DeskBook.AppServices.Services.FloorServices;
using DeskBook.AppServices.Services.UserRegistrationServices;
using DeskBook.Infrastructure.Contracts.City;
using DeskBook.Infrastructure.Contracts.FloorRepository;
using DeskBook.Infrastructure.Contracts.ITokenRepository;
using DeskBook.Infrastructure.Contracts.RestServiceClient;
using DeskBook.Infrastructure.Contracts.UserRegistration;
using DeskBook.Infrastructure.DeskbookDbContext;
using DeskBook.Infrastructure.Model.AuthoritySetting;
using DeskBook.Infrastructure.Repositories.CityRepository;
using DeskBook.Infrastructure.Repositories.FloorRepository;
using DeskBook.Infrastructure.Repositories.RestServiceClientRepository;
using DeskBook.Infrastructure.Repositories.TokenRepository;
using DeskBook.Infrastructure.Repositories.UserRegistrationRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using DeskBook.AppServices.Contracts.Seat;
using DeskBook.AppServices.Contracts.SeatBooking;
using DeskBook.AppServices.Services.SeatBooking;
using DeskBook.Infrastructure.Repositories.SeatRequest;
using DeskBook.Infrastructure.Model.EmailModel;
using DeskBook.AppServices.Services.BackGroundServices;
using DeskBook.AppServices.Contracts.UserRegistration;
using DeskBook.Infrastructure.Contracts.Seat;
using DeskBook.AppServices.Services.Seat;
using DeskBook.Infrastructure.Repositories.Seat;
using DeskBook.AppServices.Contracts.AuthorityRegistration;
using DeskBook.Infrastructure.Contracts.SeatBooking;
using DeskBook.Infrastructure.Contracts.SeatRequest;
using DeskBook.AppServices.Contracts.SeatRequest;
using DeskBook.AppServices.Services.SeatRequest;

namespace DeskBook.API
{
    public class Startup
    {
        public DeskbookConfiguration Configuration { get; }

        [System.Obsolete("This Constructor is Deprecated")]
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = new DeskbookConfiguration(builder.Build());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication("Bearer")
             .AddIdentityServerAuthentication(options =>
             {
                 var option = Configuration.Authority;
                 options.Authority = option.Authority;
                 options.RequireHttpsMetadata = option.RequireHttpsMetadata;
                 options.ApiName = option.ApiName;
                 options.ApiSecret = option.ApiSecret;
             });

            services.AddAuthorizationCore();
            services.AddSwaggerGen();

            services.AddScoped<HttpClient>();

            services.AddTransient<IFloorRepository, FloorRepository>();

            services.AddTransient<IFloorServices, FloorServices>();

            services.AddTransient<ICityRepository, CityRepository>();

            services.AddTransient<ICityServices, CitiesServices>();

            services.AddTransient<IUserRegistrationServices, UserRegistrationServices>();

            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddTransient<CustomValidationExceptionFilter>();

            services.AddTransient<ISeatRepository, SeatRepository>();

            services.AddTransient<ISeatServices, SeatServices>();

            services.AddTransient<IUserRegistrationRepository, UserRegistrationRepository>();

            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddTransient<IRestServiceClientRepository, RestServiceClientRepository>();

            services.AddTransient<IAuthorityRegistrationServices, AuthorityRegistrationServices>();

            services.AddTransient<ISeatServices, SeatServices>();

            services.AddTransient<ISeatRepository, SeatRepository>();

            services.AddTransient<ISeatBookingRepository, SeatBookingRepository>();

            services.AddTransient<ISeatBookingServices, SeatBookingServices>();

            services.AddTransient<IUserSeatRequestRepository, UserSeatRequestRepository>();

            services.AddTransient<IUserSeatRequestServices, UserSeatRequestServices>();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers();

            services.AddSingleton<DeskbookConfiguration>();

            services.AddHostedService<UserSeatRequestBackgroundServices>();

            services.AddScoped<DeskbookContext>();
            services.AddDbContext<DeskbookContext>(options
                => options.UseSqlServer(Configuration.GetConnectionString("DbString")));

            
            services.AddCors(x => x.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddSingleton(new AuthorityModel
            {
                TokenUrl = Configuration.GetSection("1Authority:AuthorityTokenUrl").Value,
                ClientId = Configuration.GetSection("1Authority:AuthorityClientId").Value,
                Secret = Configuration.GetSection("1Authority:AuthoritySecret").Value,
                BaseUri = Configuration.GetSection("1Authority:baseUri").Value
            });

            services.AddSingleton(new EmailModel
            {
                From = Configuration.GetSection("Email:From").Value,
                Password = Configuration.GetSection("Email:Password").Value,
                LogoLocation = Configuration.GetSection("Email:LogoLocation").Value
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
