namespace SmartUI.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class QueryCellInfoEventArgs<TModel>
    {
        public TModel Data { get; private set; }
        public GridRowSetting Row { get; set; } = new GridRowSetting();
        public QueryCellInfoEventArgs(TModel data) => Data = data;

    }
}
