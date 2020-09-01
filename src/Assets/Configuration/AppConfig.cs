using Assets.Configuration.Service;
using Assets.Configuration.Service.Jwt;

namespace Assets.Configuration
{
    public class AppConfig
    {
        public AssetsServiceSettings AssetsService { get; set; }

        public JwtSettings Jwt { get; set; }

        public MyNoSqlConfig MyNoSqlServer { get; set; }
    }
}
