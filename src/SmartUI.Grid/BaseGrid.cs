namespace SmartUI.Grid
{
    using SmartUI.Grid.Abstractions;

    public abstract class BaseGrid<TSource> : BaseComponent, IBaseGrid
        where TSource : class
    {
        /// <summary>
        /// Child GridColumns Component that contain all columns spacified.
        /// </summary>
        protected GridColumns gridColumns;

        public void AddGridColumns(GridColumns gridColumns)
        {
            gridColumns.GridColumnsStateChanged += StateHasChanged;
            this.gridColumns = gridColumns;

            StateHasChanged();
        } 
        protected override void Dispose(bool disposing)
        {
            gridColumns.GridColumnsStateChanged -= StateHasChanged;
            gridColumns = null;
        }
    }
}