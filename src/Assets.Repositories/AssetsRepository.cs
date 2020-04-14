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
                var entities = await context.Assets
                    .ToListAsync();

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

        public async Task<IReadOnlyList<Asset>> GetAllAsync(string brokerId, string assetId, string name, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                IQueryable<AssetEntity> query = context.Assets;

                query = query.Where(x => x.BrokerId.ToUpper() == brokerId.ToUpper());

                if (!string.IsNullOrEmpty(assetId))
                    query = query.Where(x => x.Id.Contains(assetId, StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(x => x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));

                if (isDisabled.HasValue)
                    query = query.Where(x => x.IsDisabled == isDisabled);

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

                return _mapper.Map<List<Asset>>(entities);
            }
        }

        public async Task<Asset> GetByIdAsync(string assetId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.Assets
                    .FindAsync(assetId);

                return _mapper.Map<Asset>(entity);
            }
        }

        public async Task<Asset> InsertAsync(Asset asset)
        {
            asset.Created = DateTime.UtcNow;

            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = _mapper.Map<AssetEntity>(asset);

                context.Assets.Add(entity);

                await context.SaveChangesAsync();

                return _mapper.Map<Asset>(entity);
            }
        }

        public async Task<Asset> UpdateAsync(Asset asset)
        {
            asset.Modified = DateTime.UtcNow;

            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.Assets
                    .FindAsync(asset.Id);

                _mapper.Map(asset, entity);

                await context.SaveChangesAsync();

                return _mapper.Map<Asset>(entity);
            }
        }

        public async Task DeleteAsync(string assetId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = new AssetEntity {Id = assetId};

                context.Entry(entity).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }
    }
}
