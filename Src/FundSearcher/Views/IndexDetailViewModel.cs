﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fund.Crawler.Models;
using Fund.DataBase;
using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class IndexDetailViewModel : BaseIndexViewModel
    {
        private IndexInfo indexInfo;

        #region 属性
        private ObservableCollection<FundBaseInfo> fundInfos = new ObservableCollection<FundBaseInfo>();
        public ObservableCollection<FundBaseInfo> FundInfos
        {
            get { return fundInfos; }
            set { SetProperty(ref fundInfos, value); }
        }
        #endregion

        public IndexDetailViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, eventAggregator, dataBase)
        {
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Reset, Reset);
            RegisterCommand(CommandName.Refresh, Refresh);
            RegisterCommand(CommandName.Copy, Copy);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            KeyWord = null;
            FundInfos.Clear();
            if (navigationContext.Parameters.TryGetValue(ParameterName.DetailIndexInfo, out IndexInfo info))
            {
                indexInfo = info;
                FundInfos.AddRange(info.FundBaseInfos.CustomSort());
                Filter();
            }
            PublishStatusMessage("指数详情数据加载完成");
        }

        private async void Query()
        {
            var list = await fundDataBase.GetFundBaseInfos(indexInfo);
            SetItemsSource(list.CustomSort());
            Filter();
        }

        private void Reset()
        {
            KeyWord = null;
            Query();
        }

        private async void Refresh()
        {
            var list = await fundDataBase.GetFundBaseInfos(indexInfo, true);
            SetItemsSource(list.CustomSort());
            Filter();
        }

        private void Copy()
        {
            var str = string.Join(",", FundInfos.Select(t => t.FundId));
            Clipboard.SetText(str);
            PublishStatusMessage("复制基金代码成功");
        }

        private void SetItemsSource(IEnumerable<FundBaseInfo> infos)
        {
            FundInfos.Clear();
            FundInfos.AddRange(infos);
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
