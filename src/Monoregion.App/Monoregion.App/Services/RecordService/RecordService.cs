using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monoregion.App.Entites;

namespace Monoregion.App.Services.RecordService
{
    public class RecordService : IRecordsService
    {
        public async Task<bool> AddRecordAsync(RecordModel record)
        {
            using (var context = new DatabaseContext())
            {
                context.Records.Add(record);

                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> DeleteRecordAsync(RecordModel record)
        {
            using (var context = new DatabaseContext())
            {
                context.Records.Remove(record);

                await context.SaveChangesAsync();
            }
            return true;
        }

        public Task<List<RecordModel>> GetRecordsForDirectionAsync(DirectionModel direction)
        {
            using (var context = new DatabaseContext())
            {
                return context.Records.Where(r => r.DirectionModelId == direction.Id).ToListAsync();
            }
        }

        public async Task<bool> UpdateRecordAsync(RecordModel record)
        {
            using (var context = new DatabaseContext())
            {
                context.Records.Update(record);

                await context.SaveChangesAsync();
            }
            return true;
        }
    }
}
