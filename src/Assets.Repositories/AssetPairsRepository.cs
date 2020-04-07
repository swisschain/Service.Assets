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

                query = query.Where(x => string.Equals(x.BrokerId, brokerId, StringComparison.InvariantCultureIgnoreCase));

                var entities = await query.ToListAsync();

                return _mapper.Map<List<AssetPair>>(entities);
            }
        }

        public async Task<IReadOnlyList<AssetPair>> GetAllAsync(string brokerId, string assetPairId, string name, string baseAssetId, string quoteAssetId,
            bool isDisabled = false, ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetPairEntity> query = context.AssetPairs;

                query = query.Where(x => string.Equals(x.BrokerId, brokerId, StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(assetPairId))
                    query = query.Where(assetPair => assetPair.Id.Contains(assetPairId, StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(baseAssetId))
                    query = query.Where(assetPair => assetPair.BaseAssetId.Contains(baseAssetId, StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(quoteAssetId))
                    query = query.Where(assetPair => assetPair.QuotingAssetId.Contains(quoteAssetId, StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(asset => asset.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));

                query = query.Where(asset => asset.IsDisabled == isDisabled);

                if (sortOrder == ListSortDirection.Ascending)
                {
                    if (cursor != null)
                        query = query.Where(x => String.Compare(x.Id, cursor, StringComparison.CurrentCultureIgnoreCase) >= 0);

                    query = query.OrderBy(x => x.Id);
                }
                else
                {
                    if (cursor != null)
                        query = query.Where(x => String.Compare(x.Id, cursor, StringComparison.CurrentCultureIgnoreCase) < 0);

                    query = query.OrderByDescending(x => x.Id);
                }

                query = query.Take(limit);

                var entities = await query.ToListAsync();

                return _mapper.Map<List<AssetPair>>(entities);
            }
        }

        public async Task<AssetPair> GetByIdAsync(string assetPairId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.AssetPairs
                    .FindAsync(assetPairId);

                return _mapper.Map<AssetPair>(entity);
            }
        }

        public async Task InsertAsync(AssetPair assetPair)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = _mapper.Map<AssetPairEntity>(assetPair);

                context.AssetPairs.Add(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(AssetPair assetPair)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.AssetPairs
                    .FindAsync(assetPair.Id);

                _mapper.Map(assetPair, entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string assetPairId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = new AssetPairEntity {Id = assetPairId};

                context.Entry(entity).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }
    }
}
