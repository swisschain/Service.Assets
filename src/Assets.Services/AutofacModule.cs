using Assets.Domain.Services;
using Autofac;

namespace Assets.Services
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssetPairsService>()
                .As<IAssetPairsService>()
                .SingleInstance();

            builder.RegisterType<AssetsService>()
                .As<IAssetsService>()
                .SingleInstance();
        }
    }
}
