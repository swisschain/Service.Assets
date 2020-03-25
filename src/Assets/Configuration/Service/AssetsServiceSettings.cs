using Assets.Configuration.Service.Db;

namespace Assets.Configuration.Service
{
    public class AssetsServiceSettings
    {
        public string Secret { get; set; }

        public DbSettings Db { get; set; }
    }
}
