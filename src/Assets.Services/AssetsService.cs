﻿using System;
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

        public Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId)
        {
            return _assetsRepository.GetAllAsync(brokerId);
        }

        public Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string assetId, string name, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            return _assetsRepository.GetAllAsync(brokerId, assetId, name, isDisabled, sortOrder, cursor, limit);
        }

        public Task<Asset> GetByIdAsync(string assetId)
        {
            return _assetsRepository.GetByIdAsync(assetId);
        }

        public async Task<Asset> AddAsync(string brokerId, string name, string description, int accuracy,
            bool isDisabled)
        {
            var assetId = Guid.NewGuid().ToString();

            var date = DateTime.UtcNow;

            var asset = new Asset
            {
                Id = assetId,
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

        public async Task<Asset> UpdateAsync(string assetId, string name, string description, int accuracy, bool isDisabled)
        {
            var asset = await _assetsRepository.GetByIdAsync(assetId);

            if (asset == null)
                return null;

            asset.Name = name;
            asset.Description = description;
            asset.Accuracy = accuracy;
            asset.IsDisabled = isDisabled;
            asset.Modified = DateTime.UtcNow;

            var result = await _assetsRepository.UpdateAsync(asset);

            _logger.LogInformation("Asset updated. {$Asset}", asset);

            return result;
        }

        public async Task<bool> DeleteAsync(string assetId)
        {
            var asset = await _assetsRepository.GetByIdAsync(assetId);

            if (asset == null)
                return false;

            await _assetsRepository.DeleteAsync(assetId);

            _logger.LogInformation("Asset deleted. {$Asset}", asset);

            return true;
        }
    }
}
