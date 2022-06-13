namespace SmartUI.Grid.Abstractions
{
    public interface IFilterableGrid<TColumns, TColumn> : IBaseGrid<TColumns, TColumn>
        where TColumns : IBaseGridColumns<TColumn>
        where TColumn : IBaseGridColumn
    {
        bool AllowFilter { get; set; }
    }
}

