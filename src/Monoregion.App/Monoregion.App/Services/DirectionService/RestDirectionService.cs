using Microsoft.Datasync.Client;
using Monoregion.App.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monoregion.App.Helpers;

namespace Monoregion.App.Services.DirectionService
{
    public class RestDirectionService : IDirectionService
    {
        private readonly DatasyncClient _client;

        public RestDirectionService()
        {
            _client = DatasyncClientHelper.GetDatasyncClient();
        }

        public async Task<bool> AddDirectionAsync(DirectionModel direction)
        {
            await _client.PerformOfflineTableOperation<DirectionModel>
               (t => t.InsertItemAsync(direction));

            return true;
        }

        public async Task<bool> DeleteDirectionAsync(DirectionModel direction)
        {
            await _client.PerformOfflineTableOperation<DirectionModel>
               (t => t.DeleteItemAsync(direction));

            return true;
        }

        public async Task<List<DirectionModel>> GetAllDirectionsAsync()
        {
            return await _client.PerformOfflineTableOperation<DirectionModel, List<DirectionModel>>(async t => await t.ToListAsync());
        }

        public async Task<bool> UpdateDirectionAsync(DirectionModel direction)
        {
            await _client.PerformOfflineTableOperation<DirectionModel>
               (t => t.ReplaceItemAsync(direction));

            return true;
        }
    }
}
