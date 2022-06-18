namespace SmartUI.Grid
{
    using AntiRap.Core.DynamicFilter;
    using AntiRap.Core.Utilities;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;
    using SmartUI.Grid.Abstractions;
    using SmartUI.Grid.Enums;
    using SmartUI.Grid.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class SmartGrid<TSource> : BaseOperationsGrid<TSource>, ISmartGrid
        where TSource : class
    {
        private PaginationSettings paginationSettings { get; set; }
        private TSource selectedItem;
        private Guid filterableColumnId;
        private bool showFilterPopover;
        private int greaterFilterOrder;
        private bool showSpinner;

        protected override void OnInitialized()
        {
            DataSource ??= new List<TSource>();
            AllItems = DataSource;
            paginationSettings ??= new PaginationSettings();

            base.OnInitialized();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            await GetDataSource();
        }
        private async Task OnPageSizeChange(int pageSize)
        {
            FilterationData.PageSize = pageSize;
            FilterationData.PageNumber = 1;

            await GetDataSource();
        }
        private async Task OnSelectedPage(int pageNumber)
        {
            FilterationData.PageNumber = pageNumber;

            await GetDataSource();
        }
        private void OnFilterClicked(GridColumn column)
        {
            if (filterableColumnId == column.columnId) showFilterPopover = !showFilterPopover;
            else
            {
                filterableColumnId = column.columnId;
                showFilterPopover = true;
            }
            if (FilterRules.SingleOrDefault(r => r.Id == column.columnId) == null)
                FilterRules.Add(new FilterRule<TSource>(column.columnId, column.PropertyName, PropertyHandler.GetPropertyType<TSource>(column.PropertyName), ObjectFilter.Equals));

            StateHasChanged();
        }
        private async Task ApplyFilter(Guid columnId)
        {
            FilterRule<TSource> selectedFilterRule = FilterRules?.SingleOrDefault(r => r.Id == columnId);
            if (selectedFilterRule is null)
                return;

            selectedFilterRule.IsApplied = true;
            selectedFilterRule.UpdateFilterOrder(++greaterFilterOrder);
            
            await GetDataSource();
        }
        private async Task RemoveFilter(Guid columnId)
        {
            FilterRule<TSource> selectedFilterRule = FilterRules?.SingleOrDefault(r => r.Id == columnId);
            filterableColumnId = Guid.Empty;
            if (selectedFilterRule is null)
                return;

            FilterRules.Remove(selectedFilterRule);
            
            await GetDataSource();
        }


        private async Task SortByColumn(GridColumn column)
        {
            if (string.IsNullOrEmpty(column.PropertyName) || !column.AllowSorting)
                return;

            SortDirection nextDirection = SortDirection.None;
            if (column.sortDirection == SortDirection.None) nextDirection = SortDirection.Asc;
            else if (column.sortDirection == SortDirection.Asc) nextDirection = SortDirection.Desc;

            column.UpdateSortDirection(nextDirection);

            StringBuilder orderQuery = new StringBuilder();

            foreach (GridColumn sortColumn in gridColumns?.GetAllColumns()?.Where(e => e.sortDirection != SortDirection.None) ?? new List<GridColumn>())
            {
                orderQuery.Append($"{sortColumn.PropertyName} {sortColumn.sortDirection},");
            }

            FilterationData.OrderBy = orderQuery.ToString();
            await GetDataSource();
        }

        private async Task GetDataSource()
        {
            await Loading(PerformClientSideDataManipulations);

            paginationSettings.UpdatePageSize(PaginationMetaData.PageSize);
        }
        public void AddPaginationSetting(PaginationSettings paginationSettings)
        {
            this.paginationSettings = paginationSettings;
            FilterationData.PageSize = paginationSettings.PageSize;

            StateHasChanged();
        }
        private async Task Loading(Func<Task> func)
        {
            showSpinner = true;
            StateHasChanged();

            await func();

            showSpinner = false;
            StateHasChanged();
        }
        private async Task OnRowClickedEvent(MouseEventArgs args, TSource item)
        {
            selectedItem = item;
            await RowClickedEvent.InvokeAsync(item);
        }
        public async Task GridRefresh() => await GetDataSource();
        private GridRowSetting InvokeQueryCellInfo(TSource data)
        {
            QueryCellInfoEventArgs<TSource> cellInfo = new QueryCellInfoEventArgs<TSource>(data);
            QueryCellInfo?.Invoke(cellInfo);

            return cellInfo.Row;
        }

        /// <summary>
        /// This value show when no record exist 
        /// </summary>
        [Parameter] public string NoRecordsText { get; set; } = "No Data Available";

        /// <summary>
        /// Specifies the content to be rendered inside <see cref="ChildContent"/>.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Fire when row cliecked.
        /// </summary>
        /// <param name="TSource">selected <see cref="TSource"/></param>
        [Parameter] public EventCallback<TSource> RowClickedEvent { get; set; }

        /// <summary>
        /// Get or set css class that applied on item when select it.
        /// </summary>
        [Parameter] public string SelectedItemCssClass { get; set; } = "bg-info text-white";

        /// <summary>
        /// Set table css class
        /// </summary>
        [Parameter] public string CssClass { get; set; }

        /// <summary>
        /// Set unique table id
        /// </summary>
        [Parameter] public string Id { get; set; }
        /// <summary>
        /// Table mode (Light - Dark)
        /// </summary>
        [Parameter] public TableMode Mode { get; set; } = TableMode.None;
        [Parameter] public Dictionary<string, object> RowAttributes { get; set; }
        [Parameter] public Action<QueryCellInfoEventArgs<TSource>> QueryCellInfo { get; set; }
    }
}