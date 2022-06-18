namespace SmartUI.Grid.Abstractions
{
    using System.Collections.Generic;

    public interface IBaseGridColumns
    {
        void AddColumn(GridColumn column);
        List<GridColumn> GetAllColumns();
    }
}
