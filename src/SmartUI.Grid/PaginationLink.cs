﻿namespace SmartUI.Grid
{
    public class PaginationLink
    {
        public int Page { get; set; }
        public string Text { get; set; }
        public bool Enabled { get; set; }
        public bool Active { get; set; }
        public bool IsVisible { get; set; }

        public PaginationLink(int page, bool enabled, string text)
        {
            Page = page;
            Enabled = enabled;
            Text = text;
        }
    }
}