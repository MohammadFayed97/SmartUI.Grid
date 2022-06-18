namespace SmartUI.Grid.Abstractions
{
    public interface ISmartGrid : IBaseOperationGrid
    {
       void AddPaginationSetting(PaginationSettings paginationSettings);
    }
}