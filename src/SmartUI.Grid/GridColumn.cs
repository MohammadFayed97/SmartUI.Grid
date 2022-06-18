namespace SmartUI.Grid
{
    using Microsoft.AspNetCore.Components;
    using SmartUI.Grid.Enums;
    using System;
    using System.Text;

    public class GridColumn : BaseOperationGridColumn
    {
        private bool isHidden;
        private string cssClass;
        public TextAlignment textAlignment { get; private set; } = TextAlignment.Left;

        protected override void OnInitialized()
        {
            if (GridColumns is null)
                throw new ArgumentNullException(nameof(GridColumns), $"{nameof(GridColumn)} must be include in {nameof(GridColumns)} Component");

            columnId = Guid.NewGuid();
            GridColumns.AddColumn(this);
        }

        public override string GetColumnHeaderStyle()
        {
            StringBuilder stringBuilder = new StringBuilder("padding: 0; min-width: 10vw; top: 0; vertical-align: top; border-bottom: 0;");

            string alignment = textAlignment.ToString().ToLower();
            stringBuilder.Append($"text-align:{alignment};");

            return stringBuilder.ToString();
            //@"background-color: #FFF; vertical-align: top;  padding-right: 30px;";
        }
        public override string GetColumnCellStyle() => @"text-align: left; white-space: nowrap; text-overflow: ellipsis; overflow: hidden;";

        /// <summary>
        /// Gets Sorting css classes appended to the header if AllowSorting = true.
        /// </summary>
        public string GetSortingCssClasses()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!AllowSorting)
                return string.Empty;

            stringBuilder.Append(" cursor-pointer ");
            stringBuilder.Append("sorting ");
            if (textAlignment.Equals(TextAlignment.Right))
                stringBuilder.Append("sorting-right ");
            else
                stringBuilder.Append("sorting-left ");

            if (sortDirection.Equals(SortDirection.Asc))
                stringBuilder.Append("sorting-asc ");
            else if (sortDirection.Equals(SortDirection.Desc))
                stringBuilder.Append("sorting-desc ");

            return stringBuilder.ToString();
        }
        /// <summary>
        /// Gets Full css classes appended to the header.
        /// </summary>
        public string GetFullCssClasses()
        {
            StringBuilder stringBuilder = new StringBuilder(cssClass);
            stringBuilder.Append(GetSortingCssClasses());

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Gets or sets the cascaded parent table component.
        /// </summary>
        [CascadingParameter(Name = "ParentGridColumns")] public GridColumns GridColumns { get; set; }

        /// <summary>
        /// Indicates visiblilty to column
        /// </summary>
        [Parameter]
        public bool IsHidden
        {
            get => isHidden;
            set
            {
                if (isHidden == value)
                    return;

                isHidden = value;
                RaiseStateChanged();
            }
        }

        /// <summary>
        /// Set column css class
        /// </summary>
        [Parameter] public string CssClass
        {
            get => cssClass;
            set
            {
                if (cssClass == value)
                    return;

                cssClass = value;
                RaiseStateChanged();
            }
        }

        /// <summary>
        /// Set unique column id
        /// </summary>
        [Parameter] public string Id { get; set; }
    }
}
