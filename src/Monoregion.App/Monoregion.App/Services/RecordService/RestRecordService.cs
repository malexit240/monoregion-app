using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Datasync.Client;
using Microsoft.EntityFrameworkCore;
using Monoregion.App.Entites;
using Monoregion.App.Helpers;

namespace Monoregion.App.Services.RecordService
{
    public class RestRecordService : IRecordsService
    {
        private readonly DatasyncClient _client;

        public RestRecordService()
        {
            _client = DatasyncClientHelper.GetDatasyncClient();
        }

        public async Task<RecordModel> AddRecordAsync(RecordModel record)
        {
            return await _client.PerformOfflineTableOperation<RecordModel, RecordModel>(
                async t =>
                {
                    await t.InsertItemAsync(record);
                    return record;
                });
        }

        public async Task<bool> DeleteRecordAsync(RecordModel record)
        {
            await _client.PerformOfflineTableOperation<RecordModel>(
                t =>
                {
                    return t.DeleteItemAsync(record);
                });

            return true;
        }

        public async Task<List<RecordModel>> GetRecordsForDirectionAsync(string directionId)
        {
            return await _client.PerformOfflineTableOperation<RecordModel, List<RecordModel>>(async t =>
            {
                return await t.Where(r => r.DirectionModelId == directionId).ToListAsync();
            });
        }

        public async Task<bool> UpdateRecordAsync(RecordModel record)
        {
            await _client.PerformOfflineTableOperation<RecordModel>(async t =>
            {
                await t.ReplaceItemAsync(record);
            });

            return true;
        }
    }
}
