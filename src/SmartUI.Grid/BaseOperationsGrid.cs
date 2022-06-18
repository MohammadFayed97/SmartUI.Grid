namespace SmartUI.Grid
{
    using AntiRap.Core.DynamicFilter;
    using AntiRap.Core.Pagination;
    using Microsoft.AspNetCore.Components;
    using SmartUI.Grid.Extentions;
    using SmartUI.Grid.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class BaseOperationsGrid<TSource> : BaseGrid<TSource>
        where TSource : class
    {
        protected IEnumerable<TSource> AllItems { get; set; }
        protected MetaData PaginationMetaData { get; set; } = new MetaData();
        protected List<FilterRule<TSource>> FilterRules { get; set; } = new List<FilterRule<TSource>>();
        protected FilterationData FilterationData { get; set; } = new FilterationData();

        protected Task PerformClientSideDataManipulations()
        {
            FilterationData.FilterRules = GetAppliedFilterRules();

            if (DataSource is null) DataSource = new List<TSource>();
            IQueryable<TSource> data = DataSource.AsQueryable();

            if (data.Any())
            {
                if (AllowFilter) data = data.Filter(FilterationData.FilterRules);
                if (AllowSorting) data = data.Sort(FilterationData.OrderBy);
                if (AllowPagination)
                {
                    PagedList<TSource> pagedList = data.ToPagedList(FilterationData.PageSize, FilterationData.PageNumber);
                    data = pagedList.AsQueryable();
                    PaginationMetaData = pagedList.MetaData;
                }
            }

            AllItems = data.ToList();
            
            return Task.CompletedTask;
        }
        protected IEnumerable<FilterRuleModel> GetAppliedFilterRules()
        {
            List<FilterRule<TSource>> appliedFilterRules = FilterRules?.Where(r => r.IsApplied).ToList();
            return appliedFilterRules?.Select(r => new FilterRuleModel
            {
                Field = r.PropertyName,
                Value1 = r.FilterValue1,
                Value2 = r.FilterValue2,
                FilterOperation = r.FilterType.FilterName,
                DisplayOrder = r.Order
            }).ToList();
        }
        /// <summary>
        /// spacify the DataSource to component
        /// </summary>
        [Parameter] public IEnumerable<TSource> DataSource { get; set; }
        [Parameter] public bool AllowPagination { get; set; }
        [Parameter] public bool AllowSorting { get; set; }
        [Parameter] public bool AllowFilter { get; set; }
        [Parameter] public Func<DataTableRequestArgs<TSource>, Task<PagedResponse<TSource>>> FetchData { get; set; }
    }
}