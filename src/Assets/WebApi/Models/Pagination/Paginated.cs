using System.Collections.Generic;

namespace Assets.WebApi.Models.Pagination
{
    public class Paginated<TItem, TId>
    {
        public Pagination<TId> Pagination { get; set; }
        public IReadOnlyCollection<TItem> Items { get; set; }
    }
}
