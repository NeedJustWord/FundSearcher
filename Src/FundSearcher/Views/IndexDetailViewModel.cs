using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Fund.DataBase;
using FundSearcher.Consts;
using FundSearcher.Controls;
using FundSearcher.Helpers;
using FundSearcher.PubSubEvents;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class IndexDetailViewModel : BaseIndexViewModel
    {
        private IndexInfo indexInfo;
        private bool isRefresh;

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

            isRefresh = false;
            KeyWord = null;
            FundInfos.Clear();
            if (navigationContext.Parameters.TryGetValue(ParameterName.DetailIndexInfo, out IndexInfo info))
            {
                indexInfo = info;
                FundInfos.AddRange(info.FundBaseInfos.CustomSort());
                Filter();
            }
            PublishStatusMessage($"指数[{indexInfo?.IndexCode},{indexInfo?.IndexName}]详情数据加载完成");
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            if (isRefresh) eventAggregator.Publish<IndexInfoRefreshEvent>();
        }

        private async void Query()
        {
            if (TryGetCancellationTokenFault(out var token))
            {
                MessageBoxEx.ShowError("已有任务正在执行，请等任务执行完成，或取消任务");
                return;
            }

            var task = fundDataBase.GetFundBaseInfos(indexInfo, token);
            SetRunTask(task);
            var list = await task;
            TaskCompleted();

            if (TaskIsCancel)
            {
                return;
            }

            SetItemsSource(list.CustomSort());
        }

        private void Reset()
        {
            KeyWord = null;
            Query();
        }

        private async void Refresh()
        {
            if (TryGetCancellationTokenFault(out var token))
            {
                MessageBoxEx.ShowError("已有任务正在执行，请等任务执行完成，或取消任务");
                return;
            }

            var task = fundDataBase.GetFundBaseInfos(indexInfo, token, true);
            SetRunTask(task);
            var list = await task;
            TaskCompleted();

            if (TaskIsCancel)
            {
                return;
            }

            SetItemsSource(list.CustomSort());
        }

        private void Copy()
        {
            var str = string.Join(",", FundInfos.Select(t => t.FundId));
            if (ClipboardHelper.SetText(str))
            {
                PublishStatusMessage("复制基金代码成功");
            }
            else
            {
                PublishStatusMessage("复制基金代码失败");
            }
        }

        private void SetItemsSource(IEnumerable<FundBaseInfo> infos)
        {
            isRefresh = true;
            FundInfos.Clear();
            FundInfos.AddRange(infos);
            Filter();
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
