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

    public partial class SmartGrid<TSource> : BaseOperationsGrid<TSource, GridColumns, GridColumn>, ISmartGrid
        where TSource : class
    {
        protected PaginationSettings paginationSettings;
        private TSource _selectedItem;
        private Guid _filterableColumnId;
        private bool _showFilterPopover;
        private int greaterFilterOrder;

        public override Task SetParametersAsync(ParameterView parameters)
        {
            //CssClass = "table-bordered table-striped";
            paginationSettings ??= new PaginationSettings();

            return base.SetParametersAsync(parameters);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            await GetDataSource();
        }

        private async void OnPageSizeChange(int pageSize)
        {
            _filterQuery.PageSize = pageSize;
            _filterQuery.PageNumber = 1;
            await GetDataSource();
        }
        private async void OnSelectedPage(int pageNumber)
        {
            _filterQuery.PageNumber = pageNumber;
            await GetDataSource();
        }
        private void OnFilterClicked(GridColumn column)
        {
            if (_filterableColumnId == column.columnId) _showFilterPopover = !_showFilterPopover;
            else
            {
                _filterableColumnId = column.columnId;
                _showFilterPopover = true;
            }
            if (_filterRules.SingleOrDefault(r => r.Id == column.columnId) == null)
                _filterRules.Add(new FilterRule<TSource>(column.columnId, column.PropertyName, PropertyHandler.GetPropertyType<TSource>(column.PropertyName), ObjectFilter.Equals));

            StateHasChanged();
        }
        private async Task ApplyFilter(Guid columnId)
        {
            FilterRule<TSource> selectedFilterRule = _filterRules?.SingleOrDefault(r => r.Id == columnId);
            if (selectedFilterRule is null)
                return;

            selectedFilterRule.IsApplied = true;
            selectedFilterRule.UpdateFilterOrder(++greaterFilterOrder);
            ApplyFilterOnDataSource();
        }
        private void RemoveFilter(Guid columnId)
        {
            FilterRule<TSource> selectedFilterRule = _filterRules?.SingleOrDefault(r => r.Id == columnId);
            _filterableColumnId = Guid.Empty;
            if (selectedFilterRule is null)
                return;

            _filterRules.Remove(selectedFilterRule);
            ApplyFilterOnDataSource();
        }

        private async Task OnRowClickedEvent(MouseEventArgs args, TSource item)
        {
            _selectedItem = item;
            await RowClickedEvent.InvokeAsync(item);
        }

        private async void SortByColumn(GridColumn column)
        {
            if (string.IsNullOrEmpty(column.PropertyName))
                return;

            SortDirection nextDirection = SortDirection.None;
            if (column.sortDirection == SortDirection.None) nextDirection = SortDirection.Asc;
            else if (column.sortDirection == SortDirection.Asc) nextDirection = SortDirection.Desc;

            column.UpdateSortDirection(nextDirection);

            StringBuilder orderQuery = new StringBuilder();

            foreach (GridColumn sortColumn in _gridColumns?.GetAllColumns()?.Where(e => e.sortDirection != SortDirection.None) ?? new List<GridColumn>())
            {
                orderQuery.Append($"{sortColumn.PropertyName} {sortColumn.sortDirection},");
            }

            _filterQuery.OrderBy = orderQuery.ToString();
            await GetDataSource();
        }

        public async Task GridRefresh() => await GetDataSource();
        public async Task GridRefreshWithFilterRules(IEnumerable<QueryFilterRule> queryFilterRules)
        {
            _filterQuery.FilterRules = queryFilterRules;
            await GridRefresh();
        }

        protected override async Task GetDataSource()
        {
            await base.GetDataSource();
            paginationSettings.PageSize = _responseMetaData.PageSize;
        }
        public void AddPaginationSetting(PaginationSettings paginationSettings)
        {
            this.paginationSettings = paginationSettings;
            _filterQuery.PageSize = paginationSettings.PageSize;
        }

        /// <summary>
        /// Fire when row cliecked.
        /// </summary>
        /// <param name="TSource">selected <see cref="TSource"/></param>
        [Parameter] public EventCallback<TSource> RowClickedEvent { get; set; }

        /// <summary>
        /// Get or set css class that applied on item when select it.
        /// </summary>
        [Parameter] public string SelectedItemCssClass { get; set; } = "bg-info";

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
    }
}