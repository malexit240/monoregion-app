using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monoregion.App.Entites;
using Monoregion.App.Services.EnvironmentVariableService;

namespace Monoregion.App.Services.DirectionService
{
    public class DirectionService : IDirectionService
    {
        private readonly IEnvironmentVariableService _environmentVariableService;

        public DirectionService(IEnvironmentVariableService environmentVariableService)
        {
            _environmentVariableService = environmentVariableService;
        }

        public async Task<bool> AddDirectionAsync(DirectionModel direction)
        {
            using (var context = new DatabaseContext())
            {
                context.Directions.Add(direction);

                await context.SaveChangesAsync();
            }

            await _environmentVariableService.CommitNewDBVersion();

            return true;
        }

        public async Task<bool> DeleteDirectionAsync(DirectionModel direction)
        {
            using (var context = new DatabaseContext())
            {
                context.Directions.Remove(direction);

                await context.SaveChangesAsync();
            }

            return true;
        }

        public Task<List<DirectionModel>> GetAllDirectionsAsync()
        {
            using (var context = new DatabaseContext())
            {
                return context.Directions.Include(d => d.Records).ToListAsync();
            }
        }

        public async Task<bool> UpdateDirectionAsync(DirectionModel direction)
        {
            using (var context = new DatabaseContext())
            {
                context.Directions.Update(direction);

                await context.SaveChangesAsync();
            }

            await _environmentVariableService.CommitNewDBVersion();

            return true;
        }
    }
}
