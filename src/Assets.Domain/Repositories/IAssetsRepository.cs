using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Repositories
{
    public interface IAssetsRepository
    {
        Task<IReadOnlyList<Asset>> GetAllAsync();

        Task<Asset> GetByIdAsync(string assetId);

        Task InsertAsync(Asset asset);

        Task UpdateAsync(Asset asset);

        Task DeleteAsync(string assetId);
    }
}
