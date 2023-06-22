using DeskBook.Infrastructure.Contracts.FloorRepository;
using DeskBook.Infrastructure.DeskbookDbContext;
using DeskBook.Infrastructure.Model.Floor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeskBook.Infrastructure.Repositories.FloorRepository
{
    public class FloorRepository : IFloorRepository
    {
        private readonly ILogger<FloorRepository> _logger;
        private readonly DeskbookContext _context;

        public FloorRepository(DeskbookContext context, ILogger<FloorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<FloorModel>> GetAllFloors(int cityId)
        {
            var data = await _context.FloorModels.Where(x => x.CityId == cityId).ToListAsync();
            return data;
        }
    }
}
