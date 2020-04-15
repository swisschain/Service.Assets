using Swisschain.Sdk.Server.WebApi.Pagination;

namespace Assets.WebApi.Models.AssetPairs
{
    public class AssetPairRequestMany : PaginationRequest<long>
    {
        public string Symbol { get; set; }

        public long BaseAssetId { get; set; }

        public long QuoteAssetId { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
