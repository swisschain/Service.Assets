using Assets.Domain.Repositories;
using Assets.Repositories.Context;
using Autofac;

namespace Assets.Repositories
{
    public class AutofacModule : Module
    {
        private readonly string _connectionString;

        public AutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionFactory>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_connectionString))
                .SingleInstance();

            builder.RegisterType<AssetPairsRepository>()
                .As<IAssetPairsRepository>()
                .SingleInstance();

            builder.RegisterType<AssetsRepository>()
                .As<IAssetsRepository>()
                .SingleInstance();
        }
    }
}
