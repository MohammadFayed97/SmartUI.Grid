namespace SmartUI.Grid
{
    using Microsoft.AspNetCore.Components;
    using SmartUI.Grid.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class GridColumns : BaseComponent, IBaseGridColumns
    {
        private List<GridColumn> columns = new List<GridColumn>();
        public event EventHandler GridColumnsStateChanged;

        protected override void OnInitialized()
        {
            if (Grid is null)
                throw new ArgumentNullException(nameof(Grid), $"{nameof(GridColumns)} must be include in {nameof(Grid)} Component");

            Grid.AddGridColumns(this);
        }

        /// <summary>
        /// Used to know what columns is used in <see cref="ChildContent"/>
        /// </summary>
        /// <param name="column"></param>
        public void AddColumn(GridColumn column)
        {
            column.ColumnStateChanged += StateHasChanged;

            // ToDo : enhance following
            if (!column.AllowFilter)
                column.AllowFilter = Grid.AllowFilter;
            if (!column.AllowSorting)
                column.AllowSorting = Grid.AllowSorting;

            columns.Add(column);
            
            RaiseStateChanged();
        }

        private void RaiseStateChanged() => GridColumnsStateChanged?.Invoke(this, new EventArgs());

        /// <summary>
        /// Retrive all columns used inside <see cref="ChildContent"/>
        /// </summary>
        public List<GridColumn> GetAllColumns() => columns ?? new List<GridColumn>();
        /// <summary>
        /// Retrive only visible columns
        /// </summary>
        public List<GridColumn> GetVisibleColumns() => GetAllColumns().Where(e => !e.IsHidden).ToList();



        /// <summary>
        /// Specifies the content to be rendered inside <see cref="ChildContent"/>.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The parent DataTable that inherit from <see cref="IBaseDataTable"/>
        /// </summary>
        [CascadingParameter] public ISmartGrid Grid { get; set; }
    }
}