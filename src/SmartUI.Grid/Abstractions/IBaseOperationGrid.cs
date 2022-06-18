namespace SmartUI.Grid.Abstractions
{
    public interface IBaseOperationGrid : IBaseGrid
    {
        bool AllowPagination { get; set; }
        bool AllowSorting { get; set; }
        bool AllowFilter { get; set; }
    }
}
