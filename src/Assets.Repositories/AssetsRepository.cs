using System.Collections.Generic;
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

        public async Task<Asset> GetByIdAsync(string assetId)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.Assets
                    .FindAsync(assetId);

                return _mapper.Map<Asset>(entity);
            }
        }

        public async Task InsertAsync(Asset asset)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = _mapper.Map<AssetEntity>(asset);

                context.Assets.Add(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Asset asset)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entity = await context.Assets
                    .FindAsync(asset.Id);

                _mapper.Map(asset, entity);

                await context.SaveChangesAsync();
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
