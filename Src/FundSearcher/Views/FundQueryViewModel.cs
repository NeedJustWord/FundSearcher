using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fund.Core.Extensions;
using Fund.Crawler.Models;
using Fund.DataBase;
using FundSearcher.Consts;
using FundSearcher.Controls;
using FundSearcher.Extensions;
using FundSearcher.Models;
using FundSearcher.PubSubEvents;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class FundQueryViewModel : BaseFundViewModel
    {
        private readonly string[] applyRateColumnNames = new string[] { "费率", "原费率", "天天基金优惠费率", };
        private readonly string[] buyRateColumnNames = new string[] { "费率", "原费率", "银行卡购买", "活期宝购买", };
        private readonly string[] sellRateColumnNames = new string[] { "赎回费率", };
        private readonly FundDataBase fundDataBase;
        private bool isFirstLoad = true;
        private bool filter;

        #region 属性
        private ObservableCollection<FundInfo> fundInfos = new ObservableCollection<FundInfo>();
        public ObservableCollection<FundInfo> FundInfos
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

        #region 交易场所
        private ObservableCollection<FilterModel> counters = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 交易场所
        /// </summary>
        public ObservableCollection<FilterModel> Counters
        {
            get { return counters; }
            set { SetProperty(ref counters, value); }
        }

        private FilterModel lastSelectCounter;
        private FilterModel selectCounter;
        /// <summary>
        /// 选中交易场所
        /// </summary>
        public FilterModel SelectCounter
        {
            get { return selectCounter; }
            set
            {
                if (lastSelectCounter != null && lastSelectCounter != value) lastSelectCounter.IsSelected = false;
                if (SetProperty(ref selectCounter, value) && filter)
                {
                    lastSelectCounter = value;
                    value.IsSelected = true;
                    Filter();
                }
            }
        }
        #endregion

        #region 基金类别
        private ObservableCollection<FilterModel> fundClasses = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 基金类别
        /// </summary>
        public ObservableCollection<FilterModel> FundClasses
        {
            get { return fundClasses; }
            set { SetProperty(ref fundClasses, value); }
        }

        private FilterModel lastSelectFundClass;
        private FilterModel selectFundClass;
        /// <summary>
        /// 选中基金类别
        /// </summary>
        public FilterModel SelectFundClass
        {
            get { return selectFundClass; }
            set
            {
                if (lastSelectFundClass != null && lastSelectFundClass != value) lastSelectFundClass.IsSelected = false;
                if (SetProperty(ref selectFundClass, value) && filter)
                {
                    lastSelectFundClass = value;
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
            eventAggregator.Subscribe<FundQueryCheckAllEvent>(CheckAll);
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Refresh, Refresh);
            RegisterCommand(CommandName.Compare, Compare);
            InitCounters();
            InitFundClasses();
        }

        protected override void OnLoaded()
        {
            if (isFirstLoad)
            {
                isFirstLoad = false;
                Query();
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
            var data = list.Select(t => Handle(t)).CustomSort().ToList();
            SetItemsSource(data);
            Filter();
        }

        private async void Refresh()
        {
            var fundIds = fundInfos.Where(t => t.IsShow && t.IsChecked).Select(t => t.FundId).ToArray();
            if (fundIds.Length == 0)
            {
                MessageBoxEx.ShowError("请勾选需要刷新的基金");
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
                        item.OrderNumber = FundInfos[i].OrderNumber;
                        FundInfos[i] = Handle(item).Map(FundInfos[i]);
                        eventAggregator.Publish<FundQueryRefreshDetailEvent, FundInfo>(FundInfos[i]);
                        break;
                    }
                }
            }
            Filter();
        }

        private void Compare()
        {
            var infos = fundInfos.Where(t => t.IsShow && t.IsChecked).ToArray();
            if (infos.Length < 2)
            {
                MessageBoxEx.ShowError("请至少选择2只基金进行比较");
                return;
            }
            if (infos.Length > 10)
            {
                MessageBoxEx.ShowError("最多选择10只基金进行比较");
                return;
            }

            var copyInfos = infos.Map<FundInfo, FundInfo>(t =>
            {
                t.IsChecked = false;
            });
            var param = new NavigationParameters
            {
                {ParameterName.CompareFundInfos, copyInfos},
            };
            Navigate(NavigateName.FundCompare, param);
        }

        private FundInfo Handle(FundInfo model)
        {
            var rateNames = GetRateNames(model.TransactionInfo.ApplyRates);
            model.ApplyRatesHiddenColumns = applyRateColumnNames.Except(rateNames).ToList();

            rateNames = GetRateNames(model.TransactionInfo.BuyRates);
            model.BuyRatesHiddenColumns = buyRateColumnNames.Except(rateNames).ToList();

            rateNames = GetRateNames(model.TransactionInfo.SellRates);
            model.SellRatesHiddenColumns = sellRateColumnNames.Except(rateNames).ToList();
            return model;
        }

        private List<string> GetRateNames(List<TransactionRate> list)
        {
            var result = new List<string>();
            if (list?.Count > 0)
            {
                foreach (var item in list[0].Rate.Keys)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        private void SetItemsSource(IEnumerable<FundInfo> infos)
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

        private bool IsShow(FundInfo fund)
        {
            if (SelectTrackingTarget.Key.IsNotNullAndEmpty() && fund.TrackingTarget != SelectTrackingTarget.Key) return false;
            if (SelectRunningRate.Key.IsNotNullAndEmpty() && fund.TransactionInfo.RunningRateStr != SelectRunningRate.Key) return false;
            if (SelectCounter.Key.IsNotNullAndEmpty() && fund.Counter != SelectCounter.Key) return false;
            if (SelectFundClass.Key.IsNotNullAndEmpty() && fund.FundClass != SelectFundClass.Key) return false;
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

            lastKey = isRefresh ? SelectCounter?.Key : "";
            lastSelectCounter = SelectCounter = GetDefaultSelectItem(Counters, lastKey);

            lastKey = isRefresh ? SelectFundClass?.Key : "";
            lastSelectFundClass = SelectFundClass = GetDefaultSelectItem(FundClasses, lastKey);

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

        private void InitCounters()
        {
            Counters.Clear();
            Counters.Add(new FilterModel("", "全部"));
            Counters.Add(new FilterModel("场内交易", "场内交易"));
            Counters.Add(new FilterModel("场外交易", "场外交易"));
        }

        private void InitFundClasses()
        {
            FundClasses.Clear();
            FundClasses.Add(new FilterModel("", "全部"));
            FundClasses.Add(new FilterModel("A类", "A类"));
            FundClasses.Add(new FilterModel("C类", "C类"));
        }
    }
}
