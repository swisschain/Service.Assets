using Swisschain.Sdk.Server.WebApi.Pagination;

namespace Assets.WebApi.Models.Assets
{
    public class AssetRequestMany : PaginationRequest<string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
