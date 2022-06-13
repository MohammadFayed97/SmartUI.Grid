﻿namespace SmartUI.Grid.Abstractions
{
    public interface IBaseOperationGrid<TColumns, TColumn> : IFilterableGrid<TColumns, TColumn>, ISortableGrid<TColumns, TColumn>, IPageinationGrid<TColumns, TColumn>
        where TColumns : IBaseGridColumns<TColumn>
        where TColumn : IBaseGridColumn
    {
    }
}
