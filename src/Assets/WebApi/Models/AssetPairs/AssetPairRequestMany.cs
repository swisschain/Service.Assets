namespace Assets.WebApi.Models.AssetPairs
{
    public class AssetPairRequestMany : Pagination.PaginationRequest<string>
    {
        public string Name { get; set; }

        public string AssetPairId { get; set; }

        public string BaseAssetId { get; set; }

        public string QuoteAssetId { get; set; }

        public bool IsDisabled { get; set; }
    }
}
