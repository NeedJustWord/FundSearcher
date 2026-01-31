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
        private List<string> holdingFunds;
        private bool isRefresh;
        private bool isFiltering;
        private bool isInitFilterDataFinish;
        private bool isInitCollectionFinish;
        private bool isInitTrackingTargetsFinish;
        private bool isSelectTrackingTargetChangedFinish;

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

        private FundInfo selectedFundInfo;
        /// <summary>
        /// 选中基金
        /// </summary>
        public FundInfo SelectedFundInfo
        {
            get { return selectedFundInfo; }
            set { SetProperty(ref selectedFundInfo, value); }
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
            RegisterCommand(CommandName.Detail, Detail);
            RegisterCommand(CommandName.Compare, Compare);
            RegisterCommand(CommandName.Reset, Reset);
            RegisterCommand(CommandName.Delete, Delete);
            RegisterCommand(CommandName.Add, AddBlack);
            RegisterCommand(CommandName.Black, ShowBlack);
            RegisterCommand(CommandName.HoldPosition, HoldPosition);
            RegisterCommand(CommandName.ClearPosition, ClearPosition);
            RegisterCommand(CommandName.SelectChanged, SelectChanged);
            RegisterCommand(CommandName.SelectTrackingTargetChanged, SelectTrackingTargetChanged);
            blackFunds = ConfigHelper.BlackFunds.SplitRemoveEmpty(',').ToList();
            holdingFunds = ConfigHelper.HoldingFunds.SplitRemoveEmpty(',').ToList();
        }

        protected override void OnFirstLoad()
        {
            Query();
            isSelectTrackingTargetChangedFinish = true;
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

        private void Query()
        {
            Query(false);
        }

        private async void Query(bool isRefresh)
        {
            if (KeyWordIsFocused) KeyWordIsFocused = false;

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

            var data = list.Select(t => Handle(t)).CustomSort().ToList();
            SetItemsSource(data);

            this.isRefresh = isRefresh;
            InitFilterData();
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

            isRefresh = true;
            InitFilterData();
            Filter();
        }

        private void Detail()
        {
            if (SelectedFundInfo == null)
            {
                MessageBoxEx.ShowError("请选择需要查看详情的基金");
                return;
            }

            var param = new NavigationParameters
            {
                {ParameterName.DetailFundInfo, SelectedFundInfo},
            };
            Navigate(NavigateName.FundDetail, param);
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
            SelectTrackingTarget = TrackingTargets[0];
            SelectFundClass = FundClasses[0];
            SelectCounter = Counters[0];
            SelectBuyStatus = BuyStatuses[0];
            SelectSellStatus = SellStatuses[0];
            SelectRunningRate = RunningRates[0];
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

            foreach (var item in infos)
            {
                FundInfos.Remove(item);
            }
            PublishStatusMessage(result.Count == infos.Length ? "删除成功" : "部分删除成功");

            isRefresh = true;
            InitFilterData();
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
            Query(true);
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

        private void RefreshPosition()
        {
            ConfigHelper.HoldingFunds = string.Join(",", holdingFunds.OrderBy(t => t));
            Query(true);
        }

        private void HoldPosition()
        {
            var infos = fundInfos.Where(t => t.IsShow && t.IsChecked).ToArray();
            if (infos.Length == 0)
            {
                MessageBoxEx.ShowError("请勾选持仓的基金");
                return;
            }

            foreach (var item in infos)
            {
                item.IsChecked = false;
                if (holdingFunds.Contains(item.FundId) == false) holdingFunds.Add(item.FundId);
            }

            RefreshPosition();
        }

        private void ClearPosition()
        {
            var infos = fundInfos.Where(t => t.IsShow && t.IsChecked).ToArray();
            if (infos.Length == 0)
            {
                MessageBoxEx.ShowError("请勾选清仓的基金");
                return;
            }

            foreach (var item in infos)
            {
                item.IsChecked = false;
                holdingFunds.Remove(item.FundId);
            }

            RefreshPosition();
        }

        private void SelectChanged(object[] objs)
        {
            if (isInitFilterDataFinish && isSelectTrackingTargetChangedFinish && isInitCollectionFinish)
            {
                var isInitRunningRates = objs == null || objs.Length == 0 || (bool)objs[0];
                if (isInitRunningRates)
                {
                    InitRunningRates(SelectRunningRate.Key);
                }
                Filter();
            }
        }

        private void SelectTrackingTargetChanged()
        {
            if (isInitTrackingTargetsFinish)
            {
                isSelectTrackingTargetChangedFinish = false;
                InitFundClasses(isRefresh ? SelectFundClass?.Key : "");
                InitCounters(isRefresh ? SelectCounter?.Key : "");
                InitBuyStatuses(isRefresh ? SelectBuyStatus?.Key : "");
                InitSellStatuses(isRefresh ? SelectSellStatus?.Key : "");
                InitRunningRates(isRefresh ? SelectRunningRate?.Key : "");
                isSelectTrackingTargetChangedFinish = true;
                Filter();
            }
        }

        private FundInfo Handle(FundInfo model)
        {
            model.IsHolding = holdingFunds.Contains(model.FundId);

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

        private void Filter()
        {
            if (isFiltering) return;
            isFiltering = true;

            int order = 1;
            foreach (var item in FundInfos)
            {
                item.IsShow = IsShow(item, true);
                if (item.IsShow)
                {
                    item.OrderNumber = order++;
                }
            }

            isFiltering = false;
        }

        private bool IsShow(FundInfo fund, bool checkRunningRate = false)
        {
            if (blackFunds != null && blackFunds.Contains(fund.FundId)) return false;
            if (SelectTrackingTarget != null && SelectTrackingTarget.Key.IsNotNullAndEmpty() && fund.TrackingTarget != SelectTrackingTarget.Key) return false;
            if (SelectBuyStatus != null && SelectBuyStatus.Key.IsNotNullAndEmpty() && (fund.TransactionInfo == null || fund.TransactionInfo.BuyStatus != SelectBuyStatus.Key)) return false;
            if (SelectSellStatus != null && SelectSellStatus.Key.IsNotNullAndEmpty() && (fund.TransactionInfo == null || fund.TransactionInfo.SellStatus != SelectSellStatus.Key)) return false;
            if (checkRunningRate && SelectRunningRate != null && SelectRunningRate.Key.IsNotNullAndEmpty() && (fund.TransactionInfo == null || fund.TransactionInfo.RunningRateStr != SelectRunningRate.Key)) return false;
            if (SelectCounter != null && SelectCounter.Key.IsNotNullAndEmpty() && fund.Counter != SelectCounter.Key) return false;
            if (SelectFundClass != null && SelectFundClass.Key.IsNotNullAndEmpty() && fund.FundClass != SelectFundClass.Key) return false;
            return true;
        }

        private void InitFilterData()
        {
            isInitFilterDataFinish = false;

            InitTrackingTargets(isRefresh ? SelectTrackingTarget?.Key : "");
            if (IsFirstLoad)
            {
                InitFundClasses(isRefresh ? SelectFundClass?.Key : "");
                InitCounters(isRefresh ? SelectCounter?.Key : "");
                InitBuyStatuses(isRefresh ? SelectBuyStatus?.Key : "");
                InitSellStatuses(isRefresh ? SelectSellStatus?.Key : "");
                InitRunningRates(isRefresh ? SelectRunningRate?.Key : "");
            }

            isInitFilterDataFinish = true;
        }

        private void InitTrackingTargets(string lastKey)
        {
            isInitTrackingTargetsFinish = false;
            Init(TrackingTargets, FundInfos.Where(t => IsShow(t)).Select(t => new FilterModel(t.TrackingTarget, t.TrackingTarget)));
            isInitTrackingTargetsFinish = true;
            lastSelectTrackingTarget = SelectTrackingTarget = GetDefaultSelectItem(TrackingTargets, lastKey);
        }

        private void InitRunningRates(string lastKey)
        {
            isInitCollectionFinish = false;
            Init(RunningRates, FundInfos.Where(t => t.TransactionInfo != null && IsShow(t)).Select(t => new FilterModel(t.TransactionInfo.RunningRateStr, t.TransactionInfo.RunningRate.ToString("P2"))));
            lastSelectRunningRate = SelectRunningRate = GetDefaultSelectItem(RunningRates, lastKey);
            isInitCollectionFinish = true;
        }

        private void InitBuyStatuses(string lastKey)
        {
            isInitCollectionFinish = false;
            Init(BuyStatuses, FundInfos.Where(t => t.TransactionInfo != null && IsShow(t)).Select(t => new FilterModel(t.TransactionInfo.BuyStatus, t.TransactionInfo.BuyStatus)));
            lastSelectBuyStatus = SelectBuyStatus = GetDefaultSelectItem(BuyStatuses, lastKey);
            isInitCollectionFinish = true;
        }

        private void InitSellStatuses(string lastKey)
        {
            isInitCollectionFinish = false;
            Init(SellStatuses, FundInfos.Where(t => t.TransactionInfo != null && IsShow(t)).Select(t => new FilterModel(t.TransactionInfo.SellStatus, t.TransactionInfo.SellStatus)));
            lastSelectSellStatus = SelectSellStatus = GetDefaultSelectItem(SellStatuses, lastKey);
            isInitCollectionFinish = true;
        }

        private void InitCounters(string lastKey)
        {
            isInitCollectionFinish = false;
            Init(Counters, FundInfos.Where(t => IsShow(t)).Select(t => new FilterModel(t.Counter, t.Counter)));
            lastSelectCounter = SelectCounter = GetDefaultSelectItem(Counters, lastKey);
            isInitCollectionFinish = true;
        }

        private void InitFundClasses(string lastKey)
        {
            isInitCollectionFinish = false;
            Init(FundClasses, FundInfos.Where(t => IsShow(t)).Select(t => new FilterModel(t.FundClass, t.FundClass)));
            lastSelectFundClass = SelectFundClass = GetDefaultSelectItem(FundClasses, lastKey);
            isInitCollectionFinish = true;
        }

        private void Init(ObservableCollection<FilterModel> source, IEnumerable<FilterModel> data)
        {
            source.Clear();
            source.Add(new FilterModel("", "全部"));
            source.AddRange(data.Distinct(FilterModelEqualityComparer.Instance).OrderBy(t => t.Key));
        }
    }
}
