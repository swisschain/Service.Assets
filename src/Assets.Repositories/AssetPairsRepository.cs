using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Assets.Domain.Entities;
using Assets.Domain.Repositories;
using Assets.Repositories.Context;
using Assets.Repositories.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assets.Repositories
{
    public class AssetPairsRepository : IAssetPairsRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IMapper _mapper;

        public AssetPairsRepository(ConnectionFactory connectionFactory, IMapper mapper)
        {
            _connectionFactory = connectionFactory;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AssetPair>> GetAllAsync()
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entities = await context.AssetPairs
                    .ToListAsync();

                var result = _mapper.Map<List<AssetPair>>(entities);

                var assets = await GetAllAssetsAsync(context);

                MapAssetsSymbolsToAssetPair(result, assets);

                return result;
            }
        }

        public async Task<IReadOnlyList<AssetPair>> GetAllAsync(IEnumerable<string> brokerIds)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetPairEntity> query = context.AssetPairs;

                query = query.Where(x => brokerIds.Contains(x.BrokerId));

                var entities = await query.ToListAsync();

                var result = _mapper.Map<List<AssetPair>>(entities);

                var assets = await GetAllAssetsAsync(brokerIds, context);

                MapAssetsSymbolsToAssetPair(result, assets);

                return result;
            }
        }

        public async Task<IReadOnlyList<AssetPair>> GetAllAsync(string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetPairEntity> query = context.AssetPairs;

                query = query.Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper());

                var entities = await query.ToListAsync();

                var result = _mapper.Map<AssetPair[]>(entities);

                var assets = await GetAllAssetsAsync(brokerId, context);

                MapAssetsSymbolsToAssetPair(result, assets);

                return result;
            }
        }

        public async Task<IReadOnlyList<AssetPair>> GetAllAsync(
            string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetPairEntity> query = context.AssetPairs;

                query = query.Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper());

                if (!string.IsNullOrEmpty(symbol))
                    query = query.Where(x => EF.Functions.ILike(x.Symbol, $"%{symbol}%"));

                if (isDisabled.HasValue)
                    query = query.Where(x => x.IsDisabled == isDisabled);

                if (sortOrder == ListSortDirection.Ascending)
                {
                    if (cursor != null)
                        query = query.Where(x => x.Symbol.CompareTo(cursor) >= 0);

                    query = query.OrderBy(x => x.Id);
                }
                else
                {
                    if (cursor != null)
                        query = query.Where(x => x.Symbol.CompareTo(cursor) < 0);

                    query = query.OrderByDescending(x => x.Id);
                }

                query = query.Take(limit);

                var entities = await query.ToListAsync();

                var result = _mapper.Map<List<AssetPair>>(entities);

                var assets = await GetAllAssetsAsync(brokerId, context);

                MapAssetsSymbolsToAssetPair(result, assets);

                return result;
            }
        }

        public async Task<AssetPair> GetByIdAsync(long id, string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.AssetPairs
                    .FindAsync(id);

                var result = _mapper.Map<AssetPair>(entity);

                await MapAssetsSymbolsToAssetPair(result, context);

                return result;
            }
        }

        public async Task<AssetPair> GetBySymbolAsync(string brokerId, string symbol)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await GetAsync(brokerId, symbol, context);

                if (entity == null)
                    return null;

                var result = _mapper.Map<AssetPair>(entity);

                await MapAssetsSymbolsToAssetPair(result, context);

                return result;
            }
        }

        public async Task<AssetPair> InsertAsync(AssetPair assetPair)
        {
            assetPair.Created = DateTime.UtcNow;

            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(assetPair.BrokerId, assetPair.Symbol, context);

                if (existed != null)
                    throw new InvalidOperationException($"An asset pair with the same symbol '{assetPair.Symbol}' already exists.");

                var existedBaseAsset = await GetAssetAsync(assetPair.BrokerId, assetPair.BaseAsset, context);

                if (existedBaseAsset == null)
                    throw new InvalidOperationException($"Base asset '{assetPair.BaseAsset}' not exists.");

                var existedQuoteAsset = await GetAssetAsync(assetPair.BrokerId, assetPair.QuotingAsset, context);

                if (existedQuoteAsset == null)
                    throw new InvalidOperationException($"Quote asset '{assetPair.QuotingAsset}' not exists.");

                var entity = _mapper.Map<AssetPairEntity>(assetPair);

                var now = DateTime.UtcNow;
                entity.Created = now;
                entity.Modified = now;

                context.AssetPairs.Add(entity);

                await context.SaveChangesAsync();

                var result = _mapper.Map<AssetPair>(entity);

                await MapAssetsSymbolsToAssetPair(result, context);

                return result;
            }
        }

        public async Task<AssetPair> UpdateAsync(AssetPair assetPair)
        {
            assetPair.Modified = DateTime.UtcNow;

            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(assetPair.BrokerId, assetPair.Symbol, context);

                if (existed == null)
                    throw new InvalidOperationException($"An asset pair '{assetPair.Symbol}' not exists.");

                var existedBaseAsset = await GetAssetAsync(assetPair.BrokerId, assetPair.BaseAsset, context);

                if (existedBaseAsset == null)
                    throw new InvalidOperationException($"Base asset '{assetPair.BaseAsset}' not exists.");

                var existedQuoteAsset = await GetAssetAsync(assetPair.BrokerId, assetPair.QuotingAsset, context);

                if (existedQuoteAsset == null)
                    throw new InvalidOperationException($"Quote asset '{assetPair.QuotingAsset}' not exists.");

                if (existedBaseAsset.Symbol != assetPair.BaseAsset)
                    throw new InvalidOperationException($"Base asset can't be changed from '{existedBaseAsset.Symbol}' to '{assetPair.BaseAsset}' after creation.");

                if (existedQuoteAsset.Symbol != assetPair.QuotingAsset)
                    throw new InvalidOperationException($"Quote asset can't be changed from '{existedQuoteAsset.Symbol}' to '{assetPair.QuotingAsset}' after creation.");

                _mapper.Map(assetPair, existed);

                existed.Modified = DateTime.UtcNow;

                await context.SaveChangesAsync();

                var result = _mapper.Map<AssetPair>(existed);

                await MapAssetsSymbolsToAssetPair(result, context);

                return result;
            }
        }

        public async Task DeleteAsync(long id, string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(id, brokerId, context);

                if (existed == null)
                    throw new InvalidOperationException($"Asset pair with identifier '{id}' not exists.");

                context.Entry(existed).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string brokerId, string symbol)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(brokerId, symbol, context);

                if (existed == null)
                    throw new InvalidOperationException($"Asset pair '{symbol}' not exists.");

                context.Entry(existed).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        private async Task<AssetPairEntity> GetAsync(long id, string brokerId, DataContext context)
        {
            IQueryable<AssetPairEntity> query = context.AssetPairs;

            var existed = await query
                .Where(x => x.Id == id)
                .Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper())
                .SingleOrDefaultAsync();

            return existed;
        }

        private async Task<AssetPairEntity> GetAsync(string brokerId, string symbol, DataContext context)
        {
            IQueryable<AssetPairEntity> query = context.AssetPairs;

            var existed = await query
                .Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper())
                .Where(x => x.Symbol.ToUpper() == symbol.ToUpper())
                .SingleOrDefaultAsync();

            return existed;
        }

        private async Task<AssetEntity> GetAssetAsync(long id, string brokerId, DataContext context)
        {
            IQueryable<AssetEntity> query = context.Assets;

            var existed = await query
                .Where(x => x.Id == id)
                .Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper())
                .SingleOrDefaultAsync();

            return existed;
        }

        private async Task<AssetEntity> GetAssetAsync(string brokerId, string symbol, DataContext context)
        {
            IQueryable<AssetEntity> query = context.Assets;

            var existed = await query
                .Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper())
                .Where(x => x.Symbol == symbol)
                .SingleOrDefaultAsync();

            return existed;
        }

        private async Task<IReadOnlyList<AssetEntity>> GetAllAssetsAsync(string brokerId, DataContext context)
        {
            IQueryable<AssetEntity> query = context.Assets;

            var existed = await query
                .Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper())
                .ToListAsync();

            return existed;
        }

        private async Task<IReadOnlyList<AssetEntity>> GetAllAssetsAsync(IEnumerable<string> brokerIds, DataContext context)
        {
            IQueryable<AssetEntity> query = context.Assets;

            var existed = await query
                .Where(x => brokerIds.Contains(x.BrokerId))
                .ToListAsync();

            return existed;
        }

        private async Task<IReadOnlyList<AssetEntity>> GetAllAssetsAsync(DataContext context)
        {
            IQueryable<AssetEntity> query = context.Assets;

            var existed = await query.ToListAsync();

            return existed;
        }

        private void MapAssetsSymbolsToAssetPair(IReadOnlyList<AssetPair> assetPairs, IReadOnlyList<AssetEntity> assets)
        {
            foreach (var assetPair in assetPairs)
            {
                assetPair.BaseAsset = assets.Single(x => x.Id == assetPair.BaseAssetId).Symbol;
                assetPair.QuotingAsset = assets.Single(x => x.Id == assetPair.QuotingAssetId).Symbol;
            }
        }

        private async Task MapAssetsSymbolsToAssetPair(AssetPair assetPair, DataContext context)
        {
            var baseAsset = await GetAssetAsync(assetPair.BaseAssetId, assetPair.BrokerId, context);
            var quotingAsset = await GetAssetAsync(assetPair.QuotingAssetId, assetPair.BrokerId, context);

            assetPair.BaseAsset = baseAsset.Symbol;
            assetPair.QuotingAsset = quotingAsset.Symbol;
        }
    }
}
