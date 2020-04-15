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
        private readonly IAssetsRepository _assetsRepository;
        private readonly ILogger<AssetPairsService> _logger;

        public AssetPairsService(IAssetPairsRepository assetPairsRepository,
            IAssetsRepository assetsRepository,
            ILogger<AssetPairsService> logger)
        {
            _assetPairsRepository = assetPairsRepository;
            _assetsRepository = assetsRepository;
            _logger = logger;
        }

        public Task<IReadOnlyList<AssetPair>> GetAllAsync()
        {
            return _assetPairsRepository.GetAllAsync();
        }

        public Task<IReadOnlyList<AssetPair>> GetAllAsync(IEnumerable<string> brokerIds)
        {
            return _assetPairsRepository.GetAllAsync(brokerIds);
        }

        public Task<IReadOnlyList<AssetPair>> GetAllAsync(string brokerId)
        {
            return _assetPairsRepository.GetAllAsync(brokerId);
        }

        public Task<IReadOnlyList<AssetPair>> GetAllAsync(
            string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            return _assetPairsRepository.GetAllAsync(brokerId, symbol, isDisabled, sortOrder, cursor, limit);
        }

        public Task<AssetPair> GetBySymbolAsync(string brokerId, string symbol)
        {
            return _assetPairsRepository.GetBySymbolAsync(brokerId, symbol);
        }

        public async Task<AssetPair> AddAsync(string brokerId, string symbol, string baseAsset, string quotingAsset,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled)
        {
            var baseAssetEntity = await _assetsRepository.GetBySymbolAsync(brokerId, baseAsset);

            var quotingAssetEntity = await _assetsRepository.GetBySymbolAsync(brokerId, quotingAsset);

            var assetPair = new AssetPair
            {
                BrokerId = brokerId,
                Symbol = symbol,
                BaseAssetId = baseAssetEntity.Id,
                BaseAsset = baseAssetEntity.Symbol,
                QuotingAssetId = quotingAssetEntity.Id,
                QuotingAsset = quotingAssetEntity.Symbol,
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

        public async Task<AssetPair> UpdateAsync(string brokerId, string symbol, string baseAsset, string quotingAsset,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled)
        {
            var baseAssetEntity = await _assetsRepository.GetBySymbolAsync(brokerId, baseAsset);

            var quotingAssetEntity = await _assetsRepository.GetBySymbolAsync(brokerId, quotingAsset);

            var assetPair = await _assetPairsRepository.GetBySymbolAsync(brokerId, symbol);

            if (assetPair == null)
                return null;

            assetPair.Symbol = symbol;
            assetPair.BaseAsset = baseAssetEntity.Symbol;
            assetPair.QuotingAsset = quotingAssetEntity.Symbol;
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

        public async Task<bool> DeleteAsync(string brokerId, string symbol)
        {
            var assetPair = await _assetPairsRepository.GetBySymbolAsync(brokerId, symbol);

            if (assetPair == null)
                return false;

            await _assetPairsRepository.DeleteAsync(brokerId, symbol);

            _logger.LogInformation("Asset pair deleted. {$AssetPair}", assetPair);

            return true;
        }
    }
}
