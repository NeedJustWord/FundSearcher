using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fund.Core.Consts;
using Fund.Core.Extensions;
using Fund.Core.Helpers;
using Fund.Crawler.Extensions;
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
    class FundQueryViewModel : BaseFundWithKeyWordViewModel
    {
        private readonly string[] applyRateColumnNames = new string[]
        {
            TransactionColumnName.Rates,
            TransactionColumnName.OriginalRates,
            TransactionColumnName.EastMoneyPreferredRates,
        };
        private readonly string[] buyRateColumnNames = new string[]
        {
            TransactionColumnName.Rates,
            TransactionColumnName.OriginalRates,
            TransactionColumnName.CardBuyRates,
            TransactionColumnName.CurrentBuyRates,
        };
        private readonly string[] sellRateColumnNames = new string[]
        {
            TransactionColumnName.SellRates,
        };
        private List<string> blackFunds;
        private bool isFiltering;
        private bool filter;

        #region 属性
        #region 基金列表
        private ObservableCollection<FundInfo> fundInfos = new ObservableCollection<FundInfo>();
        /// <summary>
        /// 基金列表
        /// </summary>
        public ObservableCollection<FundInfo> FundInfos
        {
            get { return fundInfos; }
            set { SetProperty(ref fundInfos, value); }
        }
        #endregion

        #region 申购状态
        private ObservableCollection<FilterModel> buyStatuses = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 申购状态
        /// </summary>
        public ObservableCollection<FilterModel> BuyStatuses
        {
            get { return buyStatuses; }
            set { SetProperty(ref buyStatuses, value); }
        }

        private FilterModel lastSelectBuyStatus;
        private FilterModel selectBuyStatus;
        /// <summary>
        /// 选中申购状态
        /// </summary>
        public FilterModel SelectBuyStatus
        {
            get { return selectBuyStatus; }
            set
            {
                SetLastUnselected(lastSelectBuyStatus, value);
                if (SetProperty(ref selectBuyStatus, value))
                {
                    lastSelectBuyStatus = value;
                    SetValueSelected(value);
                    if (filter) Filter();
                }
            }
        }
        #endregion

        #region 赎回状态
        private ObservableCollection<FilterModel> sellStatuses = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 赎回状态
        /// </summary>
        public ObservableCollection<FilterModel> SellStatuses
        {
            get { return sellStatuses; }
            set { SetProperty(ref sellStatuses, value); }
        }

        private FilterModel lastSelectSellStatus;
        private FilterModel selectSellStatus;
        /// <summary>
        /// 选中赎回状态
        /// </summary>
        public FilterModel SelectSellStatus
        {
            get { return selectSellStatus; }
            set
            {
                SetLastUnselected(lastSelectSellStatus, value);
                if (SetProperty(ref selectSellStatus, value))
                {
                    lastSelectSellStatus = value;
                    SetValueSelected(value);
                    if (filter) Filter();
                }
            }
        }
        #endregion

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
                SetLastUnselected(lastSelectTrackingTarget, value);
                if (SetProperty(ref selectTrackingTarget, value))
                {
                    lastSelectTrackingTarget = value;
                    SetValueSelected(value);
                    if (filter) Filter();
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
                SetLastUnselected(lastSelectRunningRate, value);
                if (SetProperty(ref selectRunningRate, value))
                {
                    lastSelectRunningRate = value;
                    SetValueSelected(value);
                    if (filter) Filter(false);
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
                SetLastUnselected(lastSelectCounter, value);
                if (SetProperty(ref selectCounter, value))
                {
                    lastSelectCounter = value;
                    SetValueSelected(value);
                    if (filter) Filter();
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
                SetLastUnselected(lastSelectFundClass, value);
                if (SetProperty(ref selectFundClass, value))
                {
                    lastSelectFundClass = value;
                    SetValueSelected(value);
                    if (filter) Filter();
                }
            }
        }
        #endregion
        #endregion

        public FundQueryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, eventAggregator, dataBase)
        {
            eventAggregator.Subscribe<FundQueryCheckAllEvent>(CheckAll);
            eventAggregator.Subscribe<FundBlackRefreshEvent, List<string>>(RefreshBlack);
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Refresh, Refresh);
            RegisterCommand(CommandName.Compare, Compare);
            RegisterCommand(CommandName.Reset, Reset);
            RegisterCommand(CommandName.Delete, Delete);
            RegisterCommand(CommandName.Add, AddBlack);
            RegisterCommand(CommandName.Black, ShowBlack);
            InitCounters();
            InitFundClasses();
            blackFunds = ConfigHelper.BlackFunds.SplitRemoveEmpty(',').ToList();
        }

        protected override void OnFirstLoad()
        {
            Query();
            PublishStatusMessage("基金数据加载完成");
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
            if (string.IsNullOrWhiteSpace(KeyWord))
            {
                list = fundDataBase.FundInfos;
            }
            else
            {
                if (TryGetCancellationTokenFault(out var token))
                {
                    MessageBoxEx.ShowError("已有任务正在执行，请等任务执行完成，或取消任务");
                    return;
                }

                var task = fundDataBase.GetFundInfos(KeyWord, token);
                SetRunTask(task);
                list = await task;
                TaskCompleted();

                if (TaskIsCancel)
                {
                    return;
                }
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

            if (TryGetCancellationTokenFault(out var token))
            {
                MessageBoxEx.ShowError("已有任务正在执行，请等任务执行完成，或取消任务");
                return;
            }

            var task = fundDataBase.GetFundInfos(token, true, fundIds);
            SetRunTask(task);
            var list = await task;
            TaskCompleted();

            if (TaskIsCancel)
            {
                return;
            }

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
            if (infos.Length == 0)
            {
                MessageBoxEx.ShowError("请选择需要比较的基金");
                return;
            }
            if (infos.Length > 50)
            {
                MessageBoxEx.ShowError("最多选择50只基金进行比较");
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
                MessageBoxEx.ShowError("请选择需要删除的基金");
                return;
            }

            if (MessageBoxEx.ShowQuestion("你确定要删除所选基金吗？") == MessageBoxResult.No)
            {
                PublishStatusMessage("取消删除");
                return;
            }

            var result = fundDataBase.Delete(infos);
            if (result.Count == 0)
            {
                PublishStatusMessage("删除失败");
                return;
            }

            PublishStatusMessage(result.Count == infos.Length ? "删除成功" : "部分删除成功");
            Delete(result);
        }

        private void Delete(List<FundInfo> infos)
        {
            foreach (var item in infos)
            {
                FundInfos.Remove(item);
            }

            int order = 1;
            foreach (var item in FundInfos)
            {
                if (item.IsShow)
                {
                    item.OrderNumber = order++;
                }
            }
        }

        private void RefreshBlack(List<string> blackFunds)
        {
            var cancelBlackFunds = this.blackFunds.Except(blackFunds).ToList();
            foreach (var fundId in cancelBlackFunds)
            {
                var fund = fundInfos.FirstOrDefault(t => t.FundId == fundId);
                if (fund != null) fund.IsChecked = false;
            }

            this.blackFunds = blackFunds;
            RefreshBlack();
        }

        private void RefreshBlack()
        {
            ConfigHelper.BlackFunds = string.Join(",", blackFunds.OrderBy(t => t));
            Filter(true);
        }

        private void AddBlack()
        {
            var infos = fundInfos.Where(t => t.IsShow && t.IsChecked).ToArray();
            if (infos.Length == 0)
            {
                MessageBoxEx.ShowError("请勾选需要添加黑名单的基金");
                return;
            }

            blackFunds.AddRange(infos.Select(t => t.FundId));
            RefreshBlack();
        }

        private void ShowBlack()
        {
            var infos = fundInfos.Where(t => blackFunds.Contains(t.FundId)).ToArray();
            var blackInfos = infos.Map<FundInfo, FundInfo>((t, index) =>
            {
                t.OrderNumber = index + 1;
                t.IsShow = true;
                t.IsChecked = false;
            });
            var param = new NavigationParameters
            {
                {ParameterName.BlackFundInfos, blackInfos},
            };
            Navigate(NavigateName.FundBlack, param);
        }

        private FundInfo Handle(FundInfo model)
        {
            if (model.TransactionInfo == null) return model;

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

        private void Filter(bool isInitRunningRates = true)
        {
            if (isFiltering) return;
            isFiltering = true;

            if (isInitRunningRates) InitRunningRates(SelectRunningRate.Key);

            int order = 1;
            foreach (var item in FundInfos)
            {
                item.IsShow = IsShow(item);
                if (item.IsShow)
                {
                    item.OrderNumber = order++;
                }
            }

            isFiltering = false;
        }

        private bool IsShow(FundInfo fund, bool checkRunningRate = true)
        {
            if (blackFunds.Contains(fund.FundId)) return false;
            if (SelectTrackingTarget.Key.IsNotNullAndEmpty() && fund.TrackingTarget != SelectTrackingTarget.Key) return false;
            if (SelectBuyStatus.Key.IsNotNullAndEmpty() && (fund.TransactionInfo == null || fund.TransactionInfo.BuyStatus != SelectBuyStatus.Key)) return false;
            if (SelectSellStatus.Key.IsNotNullAndEmpty() && (fund.TransactionInfo == null || fund.TransactionInfo.SellStatus != SelectSellStatus.Key)) return false;
            if (checkRunningRate && SelectRunningRate != null && SelectRunningRate.Key.IsNotNullAndEmpty() && (fund.TransactionInfo == null || fund.TransactionInfo.RunningRateStr != SelectRunningRate.Key)) return false;
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

            lastKey = isRefresh ? SelectBuyStatus?.Key : "";
            InitBuyStatuses();
            lastSelectBuyStatus = SelectBuyStatus = GetDefaultSelectItem(BuyStatuses, lastKey);

            lastKey = isRefresh ? SelectSellStatus?.Key : "";
            InitSellStatuses();
            lastSelectSellStatus = SelectSellStatus = GetDefaultSelectItem(SellStatuses, lastKey);

            lastKey = isRefresh ? SelectCounter?.Key : "";
            lastSelectCounter = SelectCounter = GetDefaultSelectItem(Counters, lastKey);

            lastKey = isRefresh ? SelectFundClass?.Key : "";
            lastSelectFundClass = SelectFundClass = GetDefaultSelectItem(FundClasses, lastKey);

            lastKey = isRefresh ? SelectRunningRate?.Key : "";
            InitRunningRates();
            lastSelectRunningRate = SelectRunningRate = GetDefaultSelectItem(RunningRates, lastKey);

            filter = true;
        }

        private void InitTrackingTargets()
        {
            TrackingTargets.Clear();
            TrackingTargets.Add(new FilterModel("", "全部"));
            TrackingTargets.AddRange(fundDataBase.FundInfos.Select(t => new FilterModel(t.TrackingTarget, t.TrackingTarget)).Distinct(FilterModelEqualityComparer.Instance));
        }

        private void InitBuyStatuses()
        {
            BuyStatuses.Clear();
            BuyStatuses.Add(new FilterModel("", "全部"));
            BuyStatuses.AddRange(fundDataBase.FundInfos.Where(t => t.TransactionInfo != null).Select(t => new FilterModel(t.TransactionInfo.BuyStatus, t.TransactionInfo.BuyStatus)).Distinct(FilterModelEqualityComparer.Instance));
        }

        private void InitSellStatuses()
        {
            SellStatuses.Clear();
            SellStatuses.Add(new FilterModel("", "全部"));
            SellStatuses.AddRange(fundDataBase.FundInfos.Where(t => t.TransactionInfo != null).Select(t => new FilterModel(t.TransactionInfo.SellStatus, t.TransactionInfo.SellStatus)).Distinct(FilterModelEqualityComparer.Instance));
        }

        private void InitRunningRates(string lastKey)
        {
            InitRunningRates();
            lastSelectRunningRate = SelectRunningRate = GetDefaultSelectItem(RunningRates, lastKey);
        }

        private void InitRunningRates()
        {
            RunningRates.Clear();
            RunningRates.Add(new FilterModel("", "全部"));
            RunningRates.AddRange(FundInfos.Where(t => t.TransactionInfo != null && IsShow(t, false)).Select(t => new FilterModel(t.TransactionInfo.RunningRateStr, t.TransactionInfo.RunningRate.ToString("P2"))).Distinct(FilterModelEqualityComparer.Instance).OrderBy(t => t.Key));
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
