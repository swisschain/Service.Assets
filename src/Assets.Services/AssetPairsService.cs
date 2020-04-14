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
    public class AssetPairsService : IAssetPairsService
    {
        private readonly IAssetPairsRepository _assetPairsRepository;
        private readonly ILogger<AssetPairsService> _logger;

        public AssetPairsService(IAssetPairsRepository assetPairsRepository, ILogger<AssetPairsService> logger)
        {
            _assetPairsRepository = assetPairsRepository;
            _logger = logger;
        }

        public Task<IReadOnlyList<AssetPair>> GetAllAsync()
        {
            return _assetPairsRepository.GetAllAsync();
        }

        public Task<IReadOnlyList<AssetPair>> GetAllAsync(string brokerId)
        {
            return _assetPairsRepository.GetAllAsync(brokerId);
        }

        public Task<IReadOnlyList<AssetPair>> GetAllAsync(
            string brokerId, string assetPairId, string name, string baseAssetId, string quoteAssetId, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            return _assetPairsRepository.GetAllAsync(brokerId, assetPairId, name, baseAssetId, quoteAssetId, isDisabled, sortOrder, cursor, limit);
        }

        public Task<AssetPair> GetByIdAsync(string assetPairId)
        {
            return _assetPairsRepository.GetByIdAsync(assetPairId);
        }

        public async Task<AssetPair> AddAsync(string brokerId, string name, string baseAssetId,
            string quotingAssetId, int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled)
        {
            var assetPairId = Guid.NewGuid().ToString();

            var date = DateTime.UtcNow;

            var assetPair = new AssetPair
            {
                Id = assetPairId,
                BrokerId = brokerId,
                Name = name,
                BaseAssetId = baseAssetId,
                QuotingAssetId = quotingAssetId,
                Accuracy = accuracy,
                MinVolume = minVolume,
                MaxVolume = maxVolume,
                MaxOppositeVolume = maxOppositeVolume,
                MarketOrderPriceThreshold = marketOrderPriceThreshold,
                IsDisabled = isDisabled,
                Created = date,
                Modified = date
            };

            var result = await _assetPairsRepository.InsertAsync(assetPair);

            _logger.LogInformation("Asset pair added. {$AssetPair}", assetPair);

            return result;
        }

        public async Task<AssetPair> UpdateAsync(string assetPairId, string name, string baseAssetId, string quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled)
        {
            var assetPair = await _assetPairsRepository.GetByIdAsync(assetPairId);

            if (assetPair == null)
                return null;

            assetPair.Name = name;
            assetPair.BaseAssetId = baseAssetId;
            assetPair.QuotingAssetId = quotingAssetId;
            assetPair.Accuracy = accuracy;
            assetPair.MinVolume = minVolume;
            assetPair.MaxVolume = maxVolume;
            assetPair.MaxOppositeVolume = maxOppositeVolume;
            assetPair.MarketOrderPriceThreshold = marketOrderPriceThreshold;
            assetPair.IsDisabled = isDisabled;
            assetPair.Modified = DateTime.UtcNow;

            var result = await _assetPairsRepository.UpdateAsync(assetPair);

            _logger.LogInformation("Asset pair updated. {$AssetPair}", assetPair);

            return result;
        }

        public async Task<bool> DeleteAsync(string assetPairId)
        {
            var assetPair = await _assetPairsRepository.GetByIdAsync(assetPairId);

            if (assetPair == null)
                return false;

            await _assetPairsRepository.DeleteAsync(assetPairId);

            _logger.LogInformation("Asset pair deleted. {$AssetPair}", assetPair);

            return true;
        }
    }
}
