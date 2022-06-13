namespace SmartUI.Grid.Abstractions
{
    public interface IBaseGrid<TColumns, TColumn>
        where TColumns : IBaseGridColumns<TColumn>
        where TColumn : IBaseGridColumn
    {
        void AddGridColumns(TColumns gridColumns);
    }
}
