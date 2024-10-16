using System.ComponentModel;
using Fund.Crawler.Models;

namespace FundSearcher.Models
{
    class FundModel : FundInfo, INotifyPropertyChanged
    {
        private bool isChecked;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked == value) return;

                isChecked = value;
                NotifyPropertyChanged(nameof(IsChecked));
            }
        }

        private bool isShow;
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return isShow; }
            set
            {
                if (isShow == value) return;

                isShow = value;
                NotifyPropertyChanged(nameof(IsShow));
            }
        }

        private int orderNumber;
        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNumber
        {
            get { return orderNumber; }
            set
            {
                if (orderNumber == value) return;

                orderNumber = value;
                NotifyPropertyChanged(nameof(OrderNumber));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
