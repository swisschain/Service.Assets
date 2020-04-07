using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetsService
    {
        Task<IReadOnlyList<Asset>> GetAllAsync();

        Task<Asset> GetByIdAsync(string assetId);

        Task<IReadOnlyList<Asset>> GetAllAsync(string name, string assetId, bool isDisabled = false,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<Asset> AddAsync(string brokerId, string name, string description, int accuracy, bool isDisabled);

        Task<bool> UpdateAsync(string assetId, string name, string description, int accuracy, bool isDisabled);

        Task<bool> DeleteAsync(string assetId);
    }
}
