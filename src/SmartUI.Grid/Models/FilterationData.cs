namespace SmartUI.Grid.Models
{
    using System.Collections.Generic;

    public class FilterationData
    {
        const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public string? OrderBy { get; set; }
        public IEnumerable<FilterRuleModel>? FilterRules { get; set; }
    }
}