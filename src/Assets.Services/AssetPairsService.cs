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
            string brokerId, string symbol, long baseAssetId, long quoteAssetId, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, long cursor = default, int limit = 50)
        {
            return _assetPairsRepository.GetAllAsync(brokerId, symbol, baseAssetId, quoteAssetId, isDisabled, sortOrder, cursor, limit);
        }

        public Task<AssetPair> GetByIdAsync(long id, string brokerId)
        {
            return _assetPairsRepository.GetByIdAsync(id, brokerId);
        }

        public async Task<AssetPair> AddAsync(string brokerId, string symbol, long baseAssetId,
            long quotingAssetId, int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled)
        {
            var assetPair = new AssetPair
            {
                BrokerId = brokerId,
                Symbol = symbol,
                BaseAssetId = baseAssetId,
                QuotingAssetId = quotingAssetId,
                Accuracy = accuracy,
                MinVolume = minVolume,
                MaxVolume = maxVolume,
                MaxOppositeVolume = maxOppositeVolume,
                MarketOrderPriceThreshold = marketOrderPriceThreshold,
                IsDisabled = isDisabled,
                Created = DateTime.UtcNow
            };

            var result = await _assetPairsRepository.InsertAsync(assetPair);

            _logger.LogInformation("Asset pair added. {$AssetPair}", result);

            return result;
        }

        public async Task<AssetPair> UpdateAsync(long id, string brokerId, string symbol, long baseAssetId, long quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled)
        {
            var assetPair = await _assetPairsRepository.GetByIdAsync(id, brokerId);

            if (assetPair == null)
                return null;

            assetPair.Id = id;
            assetPair.Symbol = symbol;
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

        public async Task<bool> DeleteAsync(long id, string brokerId)
        {
            var assetPair = await _assetPairsRepository.GetByIdAsync(id, brokerId);

            if (assetPair == null)
                return false;

            await _assetPairsRepository.DeleteAsync(id, brokerId);

            _logger.LogInformation("Asset pair deleted. {$AssetPair}", assetPair);

            return true;
        }
    }
}
