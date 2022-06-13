﻿namespace SmartUI.Grid
{
    using Microsoft.AspNetCore.Components;
    using SmartUI.Grid.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class GridColumns : BaseGridColumns<GridColumn>
    {
        protected override void OnInitialized()
        {
            if (Grid is null)
                throw new ArgumentNullException(nameof(Grid), $"{nameof(GridColumns)} must be include in {nameof(Grid)} Component");

            Grid.AddGridColumns(this);
        }

        /// <inheritdoc />
        public override void AddColumn(GridColumn column)
        {
            column.ColumnStateChanged += StateHasChanged;

            if (!column.AllowFilter)
                column.AllowFilter = Grid.AllowFilter;
            if (!column.AllowSorting)
                column.AllowSorting = Grid.AllowSorting;

            columns.Add(column);

            RaiseStateChanged();
        }
        /// <summary>
        /// The parent DataTable that inherit from <see cref="IBaseDataTable"/>
        /// </summary>
        [CascadingParameter] public IBaseOperationGrid<GridColumns, GridColumn> Grid { get; set; }
        /// <summary>
        /// Retrive only visible columns
        /// </summary>
        public List<GridColumn> GetVisibleColumns() => GetAllColumns().Where(e => !e.IsHidden).ToList();
    }
}