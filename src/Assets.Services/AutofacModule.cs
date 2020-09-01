using Assets.Domain.MyNoSql;
using Assets.Domain.Services;
using Autofac;
using MyNoSqlServer.Abstractions;

namespace Assets.Services
{
    public class AutofacModule : Module
    {
        private readonly string _myNoSqlWriterServiceUrl;

        public AutofacModule(string myNoSqlWriterServiceUrl)
        {
            _myNoSqlWriterServiceUrl = myNoSqlWriterServiceUrl;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssetPairsService>()
                .As<IAssetPairsService>()
                .SingleInstance();

            builder.RegisterType<AssetsService>()
                .As<IAssetsService>()
                .SingleInstance();


            builder.Register(ctx =>
                {
                    return new MyNoSqlServer.DataWriter.MyNoSqlServerDataWriter<AssetsEntity>(() => _myNoSqlWriterServiceUrl,
                        SetupMyNoSqlAssetService.AssetServiceTableName);
                })
                .As<IMyNoSqlServerDataWriter<AssetsEntity>>()
                .SingleInstance();

            builder.Register(ctx =>
                {
                    return new MyNoSqlServer.DataWriter.MyNoSqlServerDataWriter<AssetPairsEntity>(() => _myNoSqlWriterServiceUrl,
                        SetupMyNoSqlAssetService.AssetServiceTableName);
                })
                .As<IMyNoSqlServerDataWriter<AssetPairsEntity>>()
                .SingleInstance();
        }
    }
}
