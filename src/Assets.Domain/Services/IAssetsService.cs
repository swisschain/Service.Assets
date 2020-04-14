using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetsService
    {
        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId);

        Task<Asset> GetByIdAsync(string brokerId, string id);

        Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string id, string name, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<Asset> AddAsync(string brokerId, string id, string name, string description, int accuracy, bool isDisabled);

        Task<Asset> UpdateAsync(string brokerId, string id, string name, string description, int accuracy, bool isDisabled);

        Task DeleteAsync(string brokerId, string id);
    }
}
