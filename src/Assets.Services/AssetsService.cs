using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId)
        {
            return _assetsRepository.GetAllAsync(brokerId);
        }

        public Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string id, string name, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            return _assetsRepository.GetAllAsync(brokerId, id, name, isDisabled, sortOrder, cursor, limit);
        }

        public Task<Asset> GetByIdAsync(string brokerId, string id)
        {
            return _assetsRepository.GetByIdAsync(brokerId, id);
        }

        public async Task<Asset> AddAsync(
            string brokerId, string id, string name, string description, int accuracy, bool isDisabled)
        {
            var date = DateTime.UtcNow;

            var asset = new Asset
            {
                Id = id,
                BrokerId = brokerId,
                Name = name,
                Description = description,
                Accuracy = accuracy,
                IsDisabled = isDisabled,
                Created = date,
                Modified = date
            };

            await _assetsRepository.InsertAsync(asset);

            _logger.LogInformation("Asset added. {$Asset}", asset);

            return asset;
        }

        public async Task<Asset> UpdateAsync(string brokerId, string id, string name, string description, int accuracy, bool isDisabled)
        {
            var asset = new Asset
            {
                BrokerId = brokerId,
                Id = id,
                Name = name,
                Description = description,
                Accuracy = accuracy,
                IsDisabled = isDisabled
            };

            var result = await _assetsRepository.UpdateAsync(asset);

            _logger.LogInformation("Asset updated. {$Asset}", asset);

            return result;
        }

        public async Task DeleteAsync(string brokerId, string id)
        {
            await _assetsRepository.DeleteAsync(brokerId, id);

            _logger.LogInformation("Asset deleted. BrokerId='{$BrokerId}', AssetId='{$id}'", brokerId, id);
        }
    }
}
