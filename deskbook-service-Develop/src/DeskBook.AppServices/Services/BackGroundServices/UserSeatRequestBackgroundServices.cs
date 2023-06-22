using DeskBook.AppServices.Contracts.SeatRequest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeskBook.AppServices.Services.BackGroundServices
{
    public class UserSeatRequestBackgroundServices : BackgroundService
    {
        public IServiceProvider _serviceProvider { get; }

        public UserSeatRequestBackgroundServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await AutoApproval(cancellationToken);
        }

        private async Task AutoApproval(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IUserSeatRequestServices>();

                await scopedProcessingService.AutoApproval(cancellationToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}