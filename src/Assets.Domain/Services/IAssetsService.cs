using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetsService
    {
        Task<IReadOnlyList<Asset>> GetAllAsync();

        Task<Asset> GetByIdAsync(string assetId);

        Task<Asset> AddAsync(string assetId, string name, string description, int accuracy, bool isDisabled);

        Task UpdateAsync(string assetId, string name, string description, int accuracy, bool isDisabled);

        Task DeleteAsync(string assetId);
    }
}
