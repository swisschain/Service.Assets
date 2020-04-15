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
    public class AssetsRepository : IAssetsRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IMapper _mapper;

        public AssetsRepository(ConnectionFactory connectionFactory, IMapper mapper)
        {
            _connectionFactory = connectionFactory;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Asset>> GetAllAsync()
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetEntity> query = context.Assets;

                var entities = await query.ToListAsync();

                return _mapper.Map<List<Asset>>(entities);
            }
        }

        public async Task<IReadOnlyList<Asset>> GetAllAsync(IEnumerable<string> brokerIds)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetEntity> query = context.Assets;

                query = query.Where(x => brokerIds.Contains(x.BrokerId));

                var entities = await query.ToListAsync();

                return _mapper.Map<List<Asset>>(entities);
            }
        }

        public async Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetEntity> query = context.Assets;

                query = query.Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper());

                var entities = await query.ToListAsync();

                return _mapper.Map<List<Asset>>(entities);
            }
        }

        public async Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetEntity> query = context.Assets;

                query = query.Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper());

                if (!string.IsNullOrEmpty(symbol))
                    query = query.Where(x => EF.Functions.ILike(x.Symbol, $"%{symbol}%"));

                if (isDisabled.HasValue)
                    query = query.Where(x => x.IsDisabled == isDisabled);

                if (sortOrder == ListSortDirection.Ascending)
                {
                    if (cursor != null)
                        query = query.Where(x => x.Id.CompareTo(cursor) >= 0);

                    query = query.OrderBy(x => x.Id);
                }
                else
                {
                    if (cursor != null)
                        query = query.Where(x => x.Id.CompareTo(cursor) < 0);

                    query = query.OrderByDescending(x => x.Id);
                }

                query = query.Take(limit);

                var entities = await query.ToListAsync();

                return _mapper.Map<List<Asset>>(entities);
            }
        }

        public async Task<Asset> GetByIdAsync(long id, string brokerId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(id, brokerId, context);

                return _mapper.Map<Asset>(existed);
            }
        }

        public async Task<Asset> GetBySymbolAsync(string brokerId, string symbol)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await GetAsync(brokerId, symbol, context);

                return _mapper.Map<Asset>(entity);
            }
        }

        public async Task<Asset> InsertAsync(Asset asset)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(asset.BrokerId, asset.Symbol, context);

                if (existed != null)
                    throw new InvalidOperationException($"An asset with the same symbol '{asset.Symbol}' already exists.");

                var entity = _mapper.Map<AssetEntity>(asset);

                var now = DateTime.UtcNow;
                entity.Created = now;
                entity.Modified = now;

                context.Assets.Add(entity);

                await context.SaveChangesAsync();

                return _mapper.Map<Asset>(entity);
            }
        }

        public async Task<Asset> UpdateAsync(Asset asset)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(asset.BrokerId, asset.Symbol, context);

                if (existed == null)
                    throw new InvalidOperationException($"An asset with the identifier '{asset.Id}' not exists.");

                if (existed.Symbol != asset.Symbol)
                    throw new InvalidOperationException($"Symbol can't be changed from '{existed.Symbol}' to '{asset.Symbol}' after creation.");

                _mapper.Map(asset, existed);

                existed.Modified = DateTime.UtcNow;

                await context.SaveChangesAsync();

                return _mapper.Map<Asset>(existed);
            }
        }

        public async Task DeleteAsync(string brokerId, string symbol)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var existed = await GetAsync(brokerId, symbol, context);

                if (existed == null)
                    throw new InvalidOperationException($"An asset with the identifier '{symbol}' not exists.");

                context.Entry(existed).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        private async Task<AssetEntity> GetAsync(long id, string brokerId, DataContext context)
        {
            IQueryable<AssetEntity> query = context.Assets;

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
