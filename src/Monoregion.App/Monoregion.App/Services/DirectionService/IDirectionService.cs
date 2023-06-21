using Monoregion.App.Entites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monoregion.App.Services.DirectionService
{
    public interface IDirectionService
    {
        Task<List<DirectionModel>> GetAllDirectionsAsync();

        Task<bool> AddDirectionAsync(DirectionModel direction);

        Task<bool> UpdateDirectionAsync(DirectionModel direction);

        Task<bool> DeleteDirectionAsync(DirectionModel direction);
    }
}
