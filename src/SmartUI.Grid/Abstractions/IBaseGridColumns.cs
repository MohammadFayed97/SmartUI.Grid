namespace SmartUI.Grid.Abstractions
{
    using System.Collections.Generic;

    public interface IBaseGridColumns<TColumn>
        where TColumn : IBaseGridColumn
    {
        void AddColumn(TColumn column);
        List<TColumn> GetAllColumns();
    }
}
