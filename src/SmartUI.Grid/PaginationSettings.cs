namespace SmartUI.Grid
{
    using Microsoft.AspNetCore.Components;
    using SmartUI.Grid.Abstractions;
    using SmartUI.Grid.Enums;
    using System;
    using System.Collections.Generic;

    public class PaginationSettings : BaseComponent
    {
        protected override void OnInitialized()
        {
            if (Grid is null)
                throw new ArgumentNullException(nameof(PaginationSettings), $"{nameof(PaginationSettings)} must be include in {Grid.GetType()} Component");

            Grid.AddPaginationSetting(this);

            base.OnInitialized();
        }
        protected override void OnParametersSet()
        {
            if (LimitOfPages > 10) throw new ArgumentOutOfRangeException("Limit of page must be less than or equal 10");
            base.OnParametersSet();
        }
        
        [CascadingParameter] public ISmartGrid Grid { get; set; }

        [Parameter] public int PageSize { get; set; } = 20;
        [Parameter] public int LimitOfPages { get; set; } = 5;
        /// <summary>
        /// It Render DropDownList in the pager which allow to select PageSize from it 
        /// </summary>
        [Parameter]
        public List<int> PageSizes { get; set; } = new List<int> { 10, 20, 30, 40, 50 };
        /// <summary>
        /// Triggers when selected PageSize from DropDown
        /// </summary>
        [Parameter]
        public EventCallback<int> OnSelectedPageSize { get; set; }
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Left;

        internal void UpdatePageSize(int size) => PageSize = size;
    }
}
