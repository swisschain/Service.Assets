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
