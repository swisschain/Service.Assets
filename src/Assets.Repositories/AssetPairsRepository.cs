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

                return _mapper.Map<List<AssetPair>>(entities);
            }
        }

        public async Task<IReadOnlyList<AssetPair>> GetAllAsync(string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetEntity> query = context.Assets;

                query = query.Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper());

                var entities = await query.ToListAsync();

                return _mapper.Map<List<AssetPair>>(entities);
            }
        }

        public async Task<IReadOnlyList<AssetPair>> GetAllAsync(
            string brokerId, string symbol, long baseAssetId, long quoteAssetId, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, long cursor = default, int limit = 50)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetPairEntity> query = context.AssetPairs;

                query = query.Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper());

                if (!string.IsNullOrEmpty(symbol))
                    query = query.Where(x => EF.Functions.ILike(x.Symbol, $"%{symbol}%"));

                if (baseAssetId != default)
                    query = query.Where(assetPair => assetPair.BaseAssetId == baseAssetId);

                if (quoteAssetId != default)
                    query = query.Where(assetPair => assetPair.QuotingAssetId == quoteAssetId);

                query = query.Where(asset => asset.IsDisabled == isDisabled);

                if (sortOrder == ListSortDirection.Ascending)
                {
                    if (cursor != default)
                        query = query.Where(x => x.Id >= cursor);

                    query = query.OrderBy(x => x.Id);
                }
                else
                {
                    if (cursor != default)
                        query = query.Where(x => x.Id < cursor);

                    query = query.OrderByDescending(x => x.Id);
                }

                query = query.Take(limit);

                var entities = await query.ToListAsync();

                return _mapper.Map<List<AssetPair>>(entities);
            }
        }

        public async Task<AssetPair> GetByIdAsync(long id, string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.AssetPairs
                    .FindAsync(id);

                return _mapper.Map<AssetPair>(entity);
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

                var entity = _mapper.Map<AssetPairEntity>(assetPair);

                context.AssetPairs.Add(entity);

                await context.SaveChangesAsync();

                return _mapper.Map<AssetPair>(entity);
            }
        }

        public async Task<AssetPair> UpdateAsync(AssetPair assetPair)
        {
            assetPair.Modified = DateTime.UtcNow;

            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(assetPair.Id, assetPair.BrokerId, context);

                if (existed == null)
                    throw new InvalidOperationException($"An asset pair with the identifier '{assetPair.Id}' not exists.");

                _mapper.Map(assetPair, existed);

                await context.SaveChangesAsync();

                return _mapper.Map<AssetPair>(existed);
            }
        }

        public async Task DeleteAsync(long id, string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(id, brokerId, context);

                if (existed == null)
                    throw new InvalidOperationException($"An asset pair with the identifier '{id}' not exists.");

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

        private async Task<AssetEntity> GetAsync(string brokerId, string symbol, DataContext context)
        {
            IQueryable<AssetEntity> query = context.Assets;

            var existed = await query
                .Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper())
                .Where(x => x.Symbol.ToUpper() == symbol.ToUpper())
                .SingleOrDefaultAsync();

            return existed;
        }
    }
}
