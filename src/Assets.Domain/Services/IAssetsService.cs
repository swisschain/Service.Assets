using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetsService
    {
        Task<IReadOnlyList<Asset>> GetAllAsync();

        Task<IReadOnlyList<Asset>> GetAllAsync(IEnumerable<string> brokerIds);

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId);

        Task<Asset> GetBySymbolAsync(string brokerId, string symbol);

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<Asset> AddAsync(string brokerId, string symbol, string description, int accuracy, bool isDisabled);

        Task<Asset> UpdateAsync(string brokerId, string symbol, string description, int accuracy, bool isDisabled);

        Task<bool> DeleteAsync(string brokerId, string symbol);
    }
}
