using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Domain.Entities;
using Assets.Domain.Repositories;
using Assets.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Assets.Services
{
    public class AssetsService : IAssetsService
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly ILogger<AssetsService> _logger;

        public AssetsService(IAssetsRepository assetsRepository, ILogger<AssetsService> logger)
        {
            _assetsRepository = assetsRepository;
            _logger = logger;
        }

        public Task<IReadOnlyList<Asset>> GetAllAsync()
        {
            return _assetsRepository.GetAllAsync();
        }

        public Task<Asset> GetByIdAsync(string assetId)
        {
            return _assetsRepository.GetByIdAsync(assetId);
        }

        public async Task<Asset> AddAsync(string assetId, string name, string description, int accuracy)
        {
            var date = DateTime.UtcNow;

            var asset = new Asset
            {
                Id = assetId,
                Name = name,
                Description = description,
                Accuracy = accuracy,
                Created = date,
                Modified = date
            };

            await _assetsRepository.InsertAsync(asset);

            _logger.LogInformation("Asset added. {$Asset}", asset);

            return asset;
        }

        public async Task UpdateAsync(string assetId, string name, string description, int accuracy)
        {
            var asset = await _assetsRepository.GetByIdAsync(assetId);

            asset.Name = name;
            asset.Description = description;
            asset.Accuracy = accuracy;
            asset.Modified = DateTime.UtcNow;

            await _assetsRepository.UpdateAsync(asset);

            _logger.LogInformation("Asset updated. {$Asset}", asset);
        }

        public async Task DeleteAsync(string assetId)
        {
            var asset = await _assetsRepository.GetByIdAsync(assetId);

            await _assetsRepository.DeleteAsync(assetId);

            _logger.LogInformation("Asset deleted. {$Asset}", asset);
        }
    }
}
