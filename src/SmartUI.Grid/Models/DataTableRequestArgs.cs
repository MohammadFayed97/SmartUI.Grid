namespace SmartUI.Grid.Models
{
    using System.Collections.Generic;

    public class DataTableRequestArgs<TModel>
    {
        public FilterationData filterQuery { get; private set; } = new FilterationData();

        public DataTableRequestArgs(int pageNumber, int pageSize, List<FilterRuleModel> filterRules)
        {
            filterQuery.PageNumber = pageNumber;
            filterQuery.PageSize = pageSize;
            filterQuery.FilterRules = filterRules;
        }

        public DataTableRequestArgs(FilterationData parameters) => filterQuery = parameters;
    }
}