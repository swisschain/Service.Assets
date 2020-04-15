using Swisschain.Sdk.Server.WebApi.Pagination;

namespace Assets.WebApi.Models.AssetPairs
{
    public class AssetPairRequestMany : PaginationRequest<string>
    {
        public string Symbol { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
