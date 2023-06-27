using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public RestRecordService(DatasyncClient client)
        {
            _client = client;
        }

        public async Task<bool> AddRecordAsync(RecordModel record)
        {
            await _client.PerformOfflineTableOperation<RecordModel>(t => t.InsertItemAsync(record));

            return true;
        }

        public async Task<bool> DeleteRecordAsync(RecordModel record)
        {
            await _client.PerformOfflineTableOperation<RecordModel>(t => t.DeleteItemAsync(record));

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
                try
                {
                    await t.ReplaceItemAsync(record);
                }
                catch (Exception ex)
                {
                }
            });

            return true;
        }
    }
}
