using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetsService
    {
        Task<IReadOnlyList<Asset>> GetAllAsync();

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId);

        Task<Asset> GetByIdAsync(string assetId);

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string assetId, string name, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<Asset> AddAsync(string brokerId, string name, string description, int accuracy, bool isDisabled);

        Task<Asset> UpdateAsync(string assetId, string name, string description, int accuracy, bool isDisabled);

        Task<bool> DeleteAsync(string assetId);
    }
}
