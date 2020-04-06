namespace Assets.WebApi.Models.Assets
{
    public class AssetRequestMany : Pagination.PaginationRequest<string>
    {
        public string Name { get; set; }

        public string AssetId { get; set; }

        public bool IsDisabled { get; set; }
    }
}
