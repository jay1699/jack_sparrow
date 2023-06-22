using DeskBook.Infrastructure.Contracts.City;
using DeskBook.Infrastructure.DeskbookDbContext;
using DeskBook.Infrastructure.Model.City;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeskBook.Infrastructure.Repositories.CityRepository
{
    public class CityRepository : ICityRepository
    {
        private readonly DeskbookContext _context;
        private readonly ILogger<CityRepository> _logger;

        public CityRepository(DeskbookContext context, ILogger<CityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CityModel>> GetCityDetail()
        {
            var data = await _context.cityModels.ToListAsync();
            return data;
        }
    }
}