using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fund.Crawler.Models;
using Fund.DataBase;
using FundSearcher.Consts;
using FundSearcher.Extensions;
using FundSearcher.Models;
using FundSearcher.PubSubEvents;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class FundQueryViewModel : BaseFundViewModel
    {
        private readonly FundDataBase fundDataBase;
        private bool filter;

        #region 属性
        private ObservableCollection<FundModel> fundInfos = new ObservableCollection<FundModel>();
        public ObservableCollection<FundModel> FundInfos
        {
            get { return fundInfos; }
            set { SetProperty(ref fundInfos, value); }
        }

        private string queryFundId;
        /// <summary>
        /// 查询基金代码
        /// </summary>
        public string QueryFundId
        {
            get { return queryFundId; }
            set { SetProperty(ref queryFundId, value); }
        }

        #region 跟踪标的
        private ObservableCollection<FilterModel> trackingTargets = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 跟踪标的
        /// </summary>
        public ObservableCollection<FilterModel> TrackingTargets
        {
            get { return trackingTargets; }
            set { SetProperty(ref trackingTargets, value); }
        }

        private FilterModel lastSelectTrackingTarget;
        private FilterModel selectTrackingTarget;
        /// <summary>
        /// 选中跟踪标的
        /// </summary>
        public FilterModel SelectTrackingTarget
        {
            get { return selectTrackingTarget; }
            set
            {
                if (lastSelectTrackingTarget != null) lastSelectTrackingTarget.IsSelected = false;
                if (SetProperty(ref selectTrackingTarget, value) && filter)
                {
                    lastSelectTrackingTarget = value;
                    value.IsSelected = true;
                    Filter();
                }
            }
        }
        #endregion

        #region 运作费用
        private ObservableCollection<FilterModel> runningRates = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 运作费用
        /// </summary>
        public ObservableCollection<FilterModel> RunningRates
        {
            get { return runningRates; }
            set { SetProperty(ref runningRates, value); }
        }

        private FilterModel lastSelectRunningRate;
        private FilterModel selectRunningRate;
        /// <summary>
        /// 选中运作费用
        /// </summary>
        public FilterModel SelectRunningRate
        {
            get { return selectRunningRate; }
            set
            {
                if (lastSelectRunningRate != null) lastSelectRunningRate.IsSelected = false;
                if (SetProperty(ref selectRunningRate, value) && filter)
                {
                    lastSelectRunningRate = value;
                    value.IsSelected = true;
                    Filter();
                }
            }
        }
        #endregion
        #endregion

        public FundQueryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, eventAggregator)
        {
            fundDataBase = dataBase;
            Subscribe<FundQueryCheckAllEvent>(CheckAll);
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Refresh, Refresh);
        }

        protected override void OnLoaded()
        {
            Query();
        }

        private void CheckAll()
        {
            var value = !FundInfos.Where(t => t.IsShow).All(t => t.IsChecked);
            foreach (var item in FundInfos.Where(t => t.IsShow))
            {
                item.IsChecked = value;
            }
        }

        private async void Query()
        {
            List<FundInfo> list;
            if (string.IsNullOrWhiteSpace(queryFundId))
            {
                list = fundDataBase.FundInfos;
            }
            else
            {
                var fundInfos = fundDataBase.GetFundInfos(queryFundId);
                list = await fundInfos;
            }

            InitFilterData(false);
            var data = list.Select(t => t.Map<FundInfo, FundModel>()).OrderBy(t => t.TransactionInfo.RunningRate).ThenByDescending(t => t.AssetSize).ThenBy(t => t.BirthDay).ThenBy(t => t.FundId).ToList();
            SetItemsSource(data);
            Filter();
        }

        private async void Refresh()
        {
            var fundIds = fundInfos.Where(t => t.IsShow && t.IsChecked).Select(t => t.FundId).ToArray();
            if (fundIds.Length == 0)
            {
                MessageBox.Show("请勾选需要刷新的基金", "基金检索工具", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<FundInfo> list = await fundDataBase.GetFundInfos(true, fundIds);
            InitFilterData(true);
            foreach (var item in list)
            {
                for (int i = 0; i < FundInfos.Count; i++)
                {
                    if (FundInfos[i].FundId == item.FundId)
                    {
                        FundInfos[i] = item.Map<FundInfo, FundModel>();
                        break;
                    }
                }
            }
            Filter();
        }

        private void SetItemsSource(IEnumerable<FundModel> infos)
        {
            FundInfos.Clear();
            FundInfos.AddRange(infos);
        }

        private void Filter()
        {
            int order = 1;
            foreach (var item in FundInfos)
            {
                item.IsShow = IsShow(item);
                if (item.IsShow)
                {
                    item.OrderNumber = order++;
                }
            }
        }

        private bool IsShow(FundModel fund)
        {
            if (SelectTrackingTarget.Key.IsNotNullAndEmpty() && fund.TrackingTarget != SelectTrackingTarget.Key) return false;
            if (SelectRunningRate.Key.IsNotNullAndEmpty() && fund.TransactionInfo.RunningRateStr != SelectRunningRate.Key) return false;
            return true;
        }

        private void InitFilterData(bool isRefresh)
        {
            filter = false;

            var lastKey = isRefresh ? SelectTrackingTarget?.Key : "";
            InitTrackingTargets();
            lastSelectTrackingTarget = SelectTrackingTarget = GetDefaultSelectItem(TrackingTargets, lastKey);

            lastKey = isRefresh ? SelectRunningRate?.Key : "";
            InitRunningRates();
            lastSelectRunningRate = SelectRunningRate = GetDefaultSelectItem(RunningRates, lastKey);

            filter = true;
        }

        private FilterModel GetDefaultSelectItem(ObservableCollection<FilterModel> data, string key)
        {
            var item = key.IsNotNullAndEmpty() ? (data.FirstOrDefault(t => t.Key == key) ?? data.First()) : data.First();
            item.IsSelected = true;
            return item;
        }

        private void InitTrackingTargets()
        {
            TrackingTargets.Clear();
            TrackingTargets.Add(new FilterModel("", "全部"));
            TrackingTargets.AddRange(fundDataBase.FundInfos.Select(t => new FilterModel(t.TrackingTarget, t.TrackingTarget)).Distinct(FilterModelEqualityComparer.Instance));
        }

        private void InitRunningRates()
        {
            RunningRates.Clear();
            RunningRates.Add(new FilterModel("", "全部"));
            RunningRates.AddRange(fundDataBase.FundInfos.Select(t => new FilterModel(t.TransactionInfo.RunningRateStr, t.TransactionInfo.RunningRate.ToString("P2"))).Distinct(FilterModelEqualityComparer.Instance).OrderBy(t => t.Key));
        }
    }
}
