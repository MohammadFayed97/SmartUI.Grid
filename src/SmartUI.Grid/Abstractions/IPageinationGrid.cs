namespace SmartUI.Grid.Abstractions
{
    public interface IPageinationGrid<TColumns, TColumn> : IBaseGrid<TColumns, TColumn>
        where TColumns : IBaseGridColumns<TColumn>
        where TColumn : IBaseGridColumn
    {
        bool AllowPagination { get; set; }
    }
}
