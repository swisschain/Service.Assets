using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Repositories
{
    public interface IAssetsRepository
    {
        Task<IReadOnlyList<Asset>> GetAllAsync();

        Task<IReadOnlyList<Asset>> GetAllAsync(IEnumerable<string> brokerIds);

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId);

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, long cursor = default, int limit = 50);

        Task<Asset> GetByIdAsync(long id, string brokerId);

        Task<Asset> InsertAsync(Asset asset);

        Task<Asset> UpdateAsync(Asset asset);

        Task DeleteAsync(long id, string brokerId);
    }
}
