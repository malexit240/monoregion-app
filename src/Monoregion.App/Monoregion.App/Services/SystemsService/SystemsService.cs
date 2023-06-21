using Monoregion.App.Entites;
using System;
using System.Threading.Tasks;

namespace Monoregion.App.Services.SystemsService
{
    public class SystemsService : ISystemsService
    {
        public async Task CommitNewDBVersion()
        {
            var version = await GetAsync("DB_VERSION", "0");

            version = (int.Parse(version) + 1).ToString();

            await SetAsync("DB_VERSION", version);
        }

        public async Task<string> GetAsync(string key, string @default = "")
        {
            using (var context = new DatabaseContext())
            {
                @default = (await context.Systems.FindAsync(key))?.Value ?? @default;
            }

            return @default;
        }

        public async Task SetAsync(string key, string value)
        {
            using (var context = new DatabaseContext())
            {
                var system = await context.Systems.FindAsync(key);

                if (system == null)
                {
                    context.Systems.Add(new SystemInfoModel() { Key = key, Value = value });
                }
                else
                {
                    system.Value = value;
                    context.Systems.Update(system);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
