using System;
using System.Threading.Tasks;

namespace Monoregion.App.Services.EnvironmentVariableService
{
    public interface IEnvironmentVariableService
    {
        Task SetAsync(string key, string value);

        Task<string> GetAsync(string key, string @default = "");

        Task CommitNewDBVersion();
    }
}
