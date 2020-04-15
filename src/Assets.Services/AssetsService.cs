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

        public Task<IReadOnlyList<Asset>> GetAllAsync()
        {
            return _assetsRepository.GetAllAsync();
        }

        public Task<IReadOnlyList<Asset>> GetAllAsync(IEnumerable<string> brokerIds)
        {
            return _assetsRepository.GetAllAsync(brokerIds);
        }


        public Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId)
        {
            return _assetsRepository.GetAllAsync(brokerId);
        }

        public Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            return _assetsRepository.GetAllAsync(brokerId, symbol, isDisabled, sortOrder, cursor, limit);
        }

        public Task<Asset> GetBySymbolAsync(string brokerId, string symbol)
        {
            return _assetsRepository.GetBySymbolAsync(brokerId, symbol);
        }

        public async Task<Asset> AddAsync(
            string brokerId, string symbol, string description, int accuracy, bool isDisabled)
        {
            var date = DateTime.UtcNow;

            var asset = new Asset
            {
                BrokerId = brokerId,
                Symbol = symbol,
                Description = description,
                Accuracy = accuracy,
                IsDisabled = isDisabled,
                Created = date,
                Modified = date
            };

            var result = await _assetsRepository.InsertAsync(asset);

            _logger.LogInformation("Asset added. {$Asset}", result);

            return result;
        }

        public async Task<Asset> UpdateAsync(string brokerId, string symbol, string description, int accuracy, bool isDisabled)
        {
            var asset = new Asset
            {
                BrokerId = brokerId,
                Symbol = symbol,
                Description = description,
                Accuracy = accuracy,
                IsDisabled = isDisabled
            };

            var result = await _assetsRepository.UpdateAsync(asset);

            _logger.LogInformation("Asset updated. {$Asset}", asset);

            return result;
        }

        public async Task<bool> DeleteAsync(string brokerId, string symbol)
        {
            var asset = await _assetsRepository.GetBySymbolAsync(brokerId, symbol);

            if (asset == null)
                return false;

            await _assetsRepository.DeleteAsync(brokerId, symbol);

            _logger.LogInformation("Asset deleted. Symbol='{$symbol}'", symbol);

            return true;
        }
    }
}
