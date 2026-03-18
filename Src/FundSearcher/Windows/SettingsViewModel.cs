using System.Collections.ObjectModel;
using System.Linq;
using Fund.Core.Helpers;
using FundSearcher.Consts;
using FundSearcher.Models;

namespace FundSearcher.Windows
{
    internal class SettingsViewModel : BaseViewModel
    {
        #region 属性
        private ObservableCollection<FilterModel<bool>> yesNoSource;
        /// <summary>
        /// 是否数据源
        /// </summary>
        public ObservableCollection<FilterModel<bool>> YesNoSource
        {
            get { return yesNoSource; }
            set { SetProperty(ref yesNoSource, value); }
        }

        private FilterModel<bool> cachePageSource;
        /// <summary>
        /// 是否缓存页面源代码
        /// </summary>
        public FilterModel<bool> CachePageSource
        {
            get { return cachePageSource; }
            set
            {
                SetProperty(ref cachePageSource, value);
                Message = string.Empty;
            }
        }

        private string cacheOverDay;
        /// <summary>
        /// 页面源代码缓存过期天数
        /// </summary>
        public string CacheOverDay
        {
            get { return cacheOverDay; }
            set
            {
                SetProperty(ref cacheOverDay, value);
                Message = string.Empty;
            }
        }

        private string fundOverDay;
        /// <summary>
        /// 基金信息过期天数
        /// </summary>
        public string FundOverDay
        {
            get { return fundOverDay; }
            set
            {
                SetProperty(ref fundOverDay, value);
                Message = string.Empty;
            }
        }

        private string message;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
        #endregion

        public SettingsViewModel() : base(null, null)
        {
            YesNoSource = new ObservableCollection<FilterModel<bool>>
            {
                new FilterModel<bool>(true,"是"),
                new FilterModel<bool>(false,"否"),
            };
            RegisterCommand(CommandName.Save, Save);
            RegisterCommand(CommandName.Reset, Reset);
            Reset();
        }

        private void Save()
        {
            if (byte.TryParse(cacheOverDay, out var cacheOverDayValue) == false)
            {
                Message = "页面源代码缓存过期天数无效";
                return;
            }
            if (byte.TryParse(fundOverDay, out var fundOverDayValue) == false)
            {
                Message = "基金信息过期天数无效";
                return;
            }

            ConfigHelper.CachePageSource = CachePageSource.Key;
            ConfigHelper.CacheOverDay = cacheOverDayValue;
            ConfigHelper.FundOverDay = fundOverDayValue;
            Message = "保存成功";
        }

        private void Reset()
        {
            CachePageSource = YesNoSource.First(t => t.Key == ConfigHelper.CachePageSource);
            CacheOverDay = ConfigHelper.CacheOverDay.ToString();
            FundOverDay = ConfigHelper.FundOverDay.ToString();
        }
    }
}
