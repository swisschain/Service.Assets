using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Repositories
{
    public interface IAssetsRepository
    {
        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId);

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string id, string name, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<Asset> GetByIdAsync(string brokerId, string id);

        Task<Asset> InsertAsync(Asset asset);

        Task<Asset> UpdateAsync(Asset asset);

        Task DeleteAsync(string brokerId, string id);
    }
}
