using Swisschain.Sdk.Server.WebApi.Pagination;

namespace Assets.WebApi.Models.Assets
{
    public class AssetRequestMany : PaginationRequest<string>
    {
        public string Name { get; set; }

        public string AssetId { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
