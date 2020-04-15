using Swisschain.Sdk.Server.WebApi.Pagination;

namespace Assets.WebApi.Models.Assets
{
    public class AssetRequestMany : PaginationRequest<long>
    {
        public string Symbol { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
