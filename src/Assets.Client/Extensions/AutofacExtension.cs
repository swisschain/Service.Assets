using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace Assets.Client.Extensions
{
    /// <summary>
    /// Extension for client registration.
    /// </summary>
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers <see cref="IAssetsClient"/> in Autofac container using <see cref="AssetsClientSettings"/>.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        /// <param name="settings">Service client settings.</param>
        public static void RegisterAssetsClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] AssetsClientSettings settings)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            builder.RegisterInstance(new AssetsClient(settings))
                .As<IAssetsClient>()
                .SingleInstance();
        }
    }
}
