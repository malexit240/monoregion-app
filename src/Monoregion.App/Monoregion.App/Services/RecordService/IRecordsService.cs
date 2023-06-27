using System.Collections.Generic;
using System.Threading.Tasks;
using Monoregion.App.Entites;

namespace Monoregion.App.Services.RecordService
{
    public interface IRecordsService
    {
        Task<RecordModel> AddRecordAsync(RecordModel record);

        Task<bool> UpdateRecordAsync(RecordModel record);

        Task<bool> DeleteRecordAsync(RecordModel record);

        Task<List<RecordModel>> GetRecordsForDirectionAsync(string directionId);
    }
}
