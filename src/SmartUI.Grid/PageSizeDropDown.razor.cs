﻿namespace SmartUI.Grid
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class PageSizeDropDown : BaseComponent
    {
        /// <summary>
        /// It Render DropDownList in the pager which allow us to select PageSize from it 
        /// </summary>
        [Parameter]
        public List<int> PageSizes { get; set; }
        /// <summary>
        /// Triggers when selected PageSize from DropDown
        /// </summary>
        [Parameter]
        public EventCallback<int> OnSelectedPageSize { get; set; }
        [Parameter]
        public int? Value { get; set; }
        private async Task OnPageSizeSelected(ChangeEventArgs args)
        {
            await OnSelectedPageSize.InvokeAsync(Int32.Parse(args.Value?.ToString()));
        }
    }
}