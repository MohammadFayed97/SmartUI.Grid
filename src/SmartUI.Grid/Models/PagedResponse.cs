namespace SmartUI.Grid.Models
{
    using AntiRap.Core.Pagination;
    using System.Collections.Generic;

    public class PagedResponse<TModel> where TModel : class
    {
        public List<TModel> Items { get; set; }
        public MetaData? MetaData { get; set; }
    }
}