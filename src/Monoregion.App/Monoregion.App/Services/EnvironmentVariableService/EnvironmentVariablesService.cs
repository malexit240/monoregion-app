using Monoregion.App.Entites;
using System;
using System.Threading.Tasks;

namespace Monoregion.App.Services.EnvironmentVariableService
{
    public class EnvironmentVariablesService : IEnvironmentVariableService
    {
        public async Task CommitNewDBVersion()
        {
            //var version = await GetAsync("DB_VERSION", "0");

            //version = (int.Parse(version) + 1).ToString();

            //await SetAsync("DB_VERSION", version);
        }

        public async Task<string> GetAsync(string key, string @default = "")
        {
            return string.Empty;
            //using (var context = new DatabaseContext())
            //{
            //    @default = (await context.globalenvironmentvariable.FindAsync(key))?.Value ?? @default;
            //}

            //return @default;
        }

        public async Task SetAsync(string key, string value)
        {
            //using (var context = new DatabaseContext())
            //{
            //    var system = await context.globalenvironmentvariable.FindAsync(key);

            //    if (system == null)
            //    {
            //        context.globalenvironmentvariable.Add(new GlobalEnvironmentVariable() { Key = key, Value = value });
            //    }
            //    else
            //    {
            //        system.Value = value;
            //        context.globalenvironmentvariable.Update(system);
            //    }

            //    await context.SaveChangesAsync();
            //}
        }
    }

    public class RestEnvironmentVariablesService : IEnvironmentVariableService
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
                @default = (await context.globalenvironmentvariable.FindAsync(key))?.Value ?? @default;
            }

            return @default;
        }

        public async Task SetAsync(string key, string value)
        {
            using (var context = new DatabaseContext())
            {
                var system = await context.globalenvironmentvariable.FindAsync(key);

                if (system == null)
                {
                    context.globalenvironmentvariable.Add(new GlobalEnvironmentVariable() { Key = key, Value = value });
                }
                else
                {
                    system.Value = value;
                    context.globalenvironmentvariable.Update(system);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
