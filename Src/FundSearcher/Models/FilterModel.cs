using System.Collections.Generic;
using Prism.Mvvm;

namespace FundSearcher.Models
{
    class FilterModel : BindableBase
    {
        private bool isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        public string Key { get; set; }
        public string Value { get; set; }

        public FilterModel(string key, string value, bool isSelected = false)
        {
            Key = key;
            Value = value;
            IsSelected = isSelected;
        }
    }

    class FilterModelEqualityComparer : IEqualityComparer<FilterModel>
    {
        private static FilterModelEqualityComparer instance = new FilterModelEqualityComparer();

        public static FilterModelEqualityComparer Instance => instance;

        private FilterModelEqualityComparer()
        {
        }

        public bool Equals(FilterModel x, FilterModel y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(FilterModel obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
