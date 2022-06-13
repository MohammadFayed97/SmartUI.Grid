﻿namespace SmartUI.Grid
{
    using Microsoft.AspNetCore.Components;
    using SmartUI.Grid.Abstractions;
    using SmartUI.Grid.Enums;
    using System;

    public class PaginationSettings : BasePaginationSetting
    {
        protected override void OnInitialized()
        {
            if (Grid is null)
                throw new ArgumentNullException(nameof(PaginationSettings), $"{nameof(PaginationSettings)} must be include in {Grid.GetType()} Component");

            Grid.AddPaginationSetting(this);

            base.OnInitialized();
        }
        
        [CascadingParameter]
        public ISmartGrid Grid { get; set; }

        /// <summary>
        /// Triggers when selected PageSize from DropDown
        /// </summary>
        [Parameter]
        public EventCallback<int> OnSelectedPageSize { get; set; }
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Left;
    }
}
