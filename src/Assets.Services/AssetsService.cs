using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Assets.Domain.Entities;
using Assets.Domain.MyNoSql;
using Assets.Domain.Repositories;
using Assets.Domain.Services;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;

namespace Assets.Services
{
    public class AssetsService : IAssetsService
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly ILogger<AssetsService> _logger;
        private readonly IMyNoSqlServerDataWriter<AssetsEntity> _assetDataWriter;

        public AssetsService(IAssetsRepository assetsRepository, ILogger<AssetsService> logger, IMyNoSqlServerDataWriter<AssetsEntity> assetDataWriter)
        {
            _assetsRepository = assetsRepository;
            _logger = logger;
            _assetDataWriter = assetDataWriter;
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

            await TryUpdateMyNoSql();

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

            await TryUpdateMyNoSql();

            _logger.LogInformation("Asset updated. {$Asset}", asset);

            return result;
        }

        public async Task<bool> DeleteAsync(string brokerId, string symbol)
        {
            var asset = await _assetsRepository.GetBySymbolAsync(brokerId, symbol);

            if (asset == null)
                return false;

            await _assetsRepository.DeleteAsync(brokerId, symbol);

            await TryUpdateMyNoSql(brokerId);

            _logger.LogInformation("Asset deleted. Symbol='{$symbol}'", symbol);

            return true;
        }

        private async Task TryUpdateMyNoSql(string brokerId)
        {
            try
            {
                var assets = await _assetsRepository.GetAllAsync(brokerId);

                var entity = AssetsEntity.Generate(brokerId);
                entity.Assets.AddRange(assets);

                await _assetDataWriter.InsertOrReplaceAsync(entity);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Cannot update assets in MyNoSQL cache for broker: {brokerId}");
            }
        }

        private async Task TryUpdateMyNoSql()
        {
            var assets = await _assetsRepository.GetAllAsync();

            foreach (var data in assets.GroupBy(a => a.BrokerId))
            {
                try
                {
                    var entity = AssetsEntity.Generate(data.Key);
                    entity.Assets.AddRange(data);

                    await _assetDataWriter.InsertOrReplaceAsync(entity);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Cannot update assets in MyNoSQL cache for broker: {data.Key}");
                }
            }

            _logger.LogInformation("Finish update assets in MyNoSQL cache for ALL brokers");
        }
    }
}
