using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using FundSearcher.Consts;
using FundSearcher.Controls;
using FundSearcher.PubSubEvents;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class FundBlackViewModel : BaseFundWithKeyWordViewModel
    {
        #region 属性
        private ObservableCollection<FundBaseInfo> fundInfos = new ObservableCollection<FundBaseInfo>();
        /// <summary>
        /// 黑名单基金
        /// </summary>
        public ObservableCollection<FundBaseInfo> FundInfos
        {
            get { return fundInfos; }
            set { SetProperty(ref fundInfos, value); }
        }
        #endregion

        private bool isRefresh;

        public FundBlackViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            eventAggregator.Subscribe<FundBlackCheckAllEvent>(CheckAll);
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Reset, Reset);
            RegisterCommand(CommandName.Delete, Delete);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            isRefresh = false;
            KeyWord = null;
            FundInfos.Clear();
            if (navigationContext.Parameters.TryGetValue(ParameterName.BlackFundInfos, out FundInfo[] infos))
            {
                FundInfos.AddRange(infos);
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            if (isRefresh)
            {
                eventAggregator.Publish<FundBlackRefreshEvent, List<string>>(fundInfos.Select(t => t.FundId).ToList());
            }
        }

        private void CheckAll()
        {
            var value = !FundInfos.Where(t => t.IsShow).All(t => t.IsChecked);
            foreach (var item in FundInfos.Where(t => t.IsShow))
            {
                item.IsChecked = value;
            }
        }

        private void Query()
        {
            if (KeyWordIsFocused) KeyWordIsFocused = false;

            Filter();
        }

        private void Reset()
        {
            KeyWord = null;
            Query();
        }

        private void Delete()
        {
            var infos = fundInfos.Where(t => t.IsShow && t.IsChecked).ToArray();
            if (infos.Length == 0)
            {
                MessageBoxEx.ShowError("请勾选需要取消黑名单的基金");
                return;
            }

            foreach (var item in infos)
            {
                fundInfos.Remove(item);
            }

            int order = 1;
            foreach (var item in FundInfos)
            {
                if (item.IsShow)
                {
                    item.OrderNumber = order++;
                }
            }

            isRefresh = true;
        }

        private void Filter()
        {
            var keyWords = KeyWord.InputSplit();

            int order = 1;
            foreach (var item in FundInfos)
            {
                item.IsShow = IsShow(item, keyWords);
                if (item.IsShow)
                {
                    item.OrderNumber = order++;
                }
            }
        }

        private bool IsShow(FundBaseInfo index, string[] keyWords)
        {
            if (keyWords.Length == 0) return true;
            return keyWords.Any(t => index.FundId.Contains(t) || index.FundName.Contains(t));
        }
    }
}
