using System;
using System.Threading.Tasks;

namespace Monoregion.App.Services.SystemsService
{
    public interface ISystemsService
    {
        Task SetAsync(string key, string value);

        Task<string> GetAsync(string key, string @default = "");

        Task CommitNewDBVersion();
    }
}
