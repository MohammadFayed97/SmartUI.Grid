﻿namespace SmartUI.Grid.Abstractions
{
    public interface ISortableGrid<TColumns, TColumn> : IBaseGrid<TColumns, TColumn>
        where TColumns : IBaseGridColumns<TColumn>
        where TColumn : IBaseGridColumn
    {
        bool AllowSorting { get; set; }
    }
}
