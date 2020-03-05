using System;
using System.Collections.Generic;
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

        public Task<AssetPair> GetByIdAsync(string assetPairId)
        {
            return _assetPairsRepository.GetByIdAsync(assetPairId);
        }

        public async Task<AssetPair> AddAsync(string assetPairId, string name, string baseAssetId,
            string quotingAssetId, int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold)
        {
            var date = DateTime.UtcNow;

            var assetPair = new AssetPair
            {
                Id = assetPairId,
                Name = name,
                BaseAssetId = baseAssetId,
                QuotingAssetId = quotingAssetId,
                Accuracy = accuracy,
                MinVolume = minVolume,
                MaxVolume = maxVolume,
                MaxOppositeVolume = maxOppositeVolume,
                MarketOrderPriceThreshold = marketOrderPriceThreshold,
                Created = date,
                Modified = date
            };

            await _assetPairsRepository.InsertAsync(assetPair);

            _logger.LogInformation("Asset pair added. {$AssetPair}", assetPair);

            return assetPair;
        }

        public async Task UpdateAsync(string assetPairId, string name, string baseAssetId, string quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold)
        {
            var assetPair = await _assetPairsRepository.GetByIdAsync(assetPairId);

            assetPair.Name = name;
            assetPair.BaseAssetId = baseAssetId;
            assetPair.QuotingAssetId = quotingAssetId;
            assetPair.Accuracy = accuracy;
            assetPair.MinVolume = minVolume;
            assetPair.MaxVolume = maxVolume;
            assetPair.MaxOppositeVolume = maxOppositeVolume;
            assetPair.MarketOrderPriceThreshold = marketOrderPriceThreshold;
            assetPair.Modified = DateTime.UtcNow;

            await _assetPairsRepository.UpdateAsync(assetPair);

            _logger.LogInformation("Asset pair updated. {$AssetPair}", assetPair);
        }

        public async Task DeleteAsync(string assetPairId)
        {
            var assetPair = await _assetPairsRepository.GetByIdAsync(assetPairId);

            await _assetPairsRepository.DeleteAsync(assetPairId);

            _logger.LogInformation("Asset pair deleted. {$AssetPair}", assetPair);
        }
    }
}
