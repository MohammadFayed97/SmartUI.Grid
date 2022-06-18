namespace SmartUI.Grid
{
    using System.Text;

    public class GridRowSetting
    {
        private string[] _cssClass;
        private StringBuilder _text = new StringBuilder();
        public void AddClass(string[] cssClass) => _cssClass = cssClass;

        internal string GetCssClass()
        {
            if (_cssClass == null)
                return string.Empty;

            _text.Clear();
            foreach (var item in _cssClass)
            {
                _text.Append(item);
                _text.Append(" ");
            }
            return _text.ToString();
        }
    }
}