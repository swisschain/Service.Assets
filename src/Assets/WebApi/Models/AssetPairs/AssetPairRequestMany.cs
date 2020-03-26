namespace Assets.WebApi.Models.AssetPairs
{
    public class AssetPairRequestMany : Pagination.PaginationRequest<string>
    {
        public string Name { get; set; }

        public string AssetPairId { get; set; }
    }
}
