using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fund.Core.Extensions;
using Fund.Core.Helpers;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Fund.DataBase;
using FundSearcher.Consts;
using FundSearcher.Controls;
using FundSearcher.Helpers;
using FundSearcher.Models;
using FundSearcher.PubSubEvents;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class IndexQueryViewModel : BaseIndexViewModel
    {
        #region 属性
        private IndexInfo selectedIndexInfo;
        /// <summary>
        /// 选中指数信息
        /// </summary>
        public IndexInfo SelectedIndexInfo
        {
            get { return selectedIndexInfo; }
            set { SetProperty(ref selectedIndexInfo, value); }
        }

        private ObservableCollection<IndexInfo> indexInfos = new ObservableCollection<IndexInfo>();
        /// <summary>
        /// 指数信息集合
        /// </summary>
        public ObservableCollection<IndexInfo> IndexInfos
        {
            get { return indexInfos; }
            set { SetProperty(ref indexInfos, value); }
        }

        #region 关注指数
        private ObservableCollection<FilterModel> starIndexes = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 关注指数
        /// </summary>
        public ObservableCollection<FilterModel> StarIndexes
        {
            get { return starIndexes; }
            set { SetProperty(ref starIndexes, value); }
        }

        private FilterModel lastSelectStarIndex;
        private FilterModel selectStarIndex;
        /// <summary>
        /// 选中关注指数
        /// </summary>
        public FilterModel SelectStarIndex
        {
            get { return selectStarIndex; }
            set
            {
                SetLastUnselected(lastSelectStarIndex, value);
                if (SetProperty(ref selectStarIndex, value))
                {
                    lastSelectStarIndex = value;
                    SetValueSelected(value);
                }
            }
        }
        #endregion

        #endregion

        private readonly List<string> starIndexCodes;

        public IndexQueryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, eventAggregator, dataBase)
        {
            eventAggregator.Subscribe<IndexQueryCheckAllEvent>(CheckAll);
            eventAggregator.Subscribe<IndexInfoRefreshEvent>(IndexInfoRefresh);
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Reset, Reset);
            RegisterCommand(CommandName.Refresh, Refresh);
            RegisterCommand(CommandName.Detail, Detail);
            RegisterCommand(CommandName.Add, Add);
            RegisterCommand(CommandName.Delete, Delete);
            RegisterCommand(CommandName.Copy, Copy);
            RegisterCommand(CommandName.RefreshDetail, RefreshDetail);
            RegisterCommand(CommandName.SelectChanged, SelectChanged);
            starIndexCodes = ConfigHelper.StarIndexes.SplitRemoveEmpty(',').ToList();
        }

        protected override void OnFirstLoad()
        {
            Query();
            PublishStatusMessage("指数数据加载完成");
        }

        private void CheckAll()
        {
            var value = !IndexInfos.Where(t => t.IsShow).All(t => t.IsChecked);
            foreach (var item in IndexInfos.Where(t => t.IsShow))
            {
                item.IsChecked = value;
            }
        }

        private void IndexInfoRefresh()
        {
            var lastSelected = SelectedIndexInfo;
            SetItemsSource(true, indexInfos.CustomSort().ToList());
            SelectedIndexInfo = lastSelected;
        }

        private async void Query()
        {
            if (KeyWordIsFocused) KeyWordIsFocused = false;

            if (TryGetCancellationTokenFault(out var token))
            {
                MessageBoxEx.ShowError("已有任务正在执行，请等任务执行完成，或取消任务");
                return;
            }

            var task = fundDataBase.GetIndexInfos(token);
            SetRunTask(task);
            var list = await task;
            TaskCompleted();

            if (TaskIsCancel)
            {
                return;
            }

            SetItemsSource(false, list.CustomSort());
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

            var task = fundDataBase.GetIndexInfos(token, true);
            SetRunTask(task);
            var list = await task;
            TaskCompleted();

            if (TaskIsCancel)
            {
                return;
            }

            SetItemsSource(true, list.CustomSort());
        }

        private void Detail()
        {
            if (SelectedIndexInfo == null)
            {
                MessageBoxEx.ShowError("请选择需要查看详情的指数");
                return;
            }

            var param = new NavigationParameters
            {
                {ParameterName.DetailIndexInfo, SelectedIndexInfo},
            };
            Navigate(NavigateName.IndexDetail, param);
        }

        private void Add()
        {
            var checkInfos = indexInfos.Where(t => t.IsShow && t.IsChecked).ToArray();
            if (checkInfos.Length == 0)
            {
                MessageBoxEx.ShowError("请勾选需要关注的指数");
                return;
            }

            foreach (var info in checkInfos)
            {
                var temp = StarIndexes.FirstOrDefault(t => t.Key == info.IndexCode);
                if (temp == null)
                {
                    StarIndexes.Add(new FilterModel(info.IndexCode, info.IndexName));
                }

                if (starIndexCodes.FindIndex(t => t == info.IndexCode) == -1)
                {
                    starIndexCodes.Add(info.IndexCode);
                }
            }

            SaveStarIndexes();
        }

        private void Delete()
        {
            var checkInfos = indexInfos.Where(t => t.IsShow && t.IsChecked).ToArray();
            if (checkInfos.Length == 0)
            {
                MessageBoxEx.ShowError("请勾选需要取关的指数");
                return;
            }

            string indexCode;
            for (int i = StarIndexes.Count - 1; i >= 0; i--)
            {
                indexCode = StarIndexes[i].Key;
                var temp = checkInfos.FirstOrDefault(t => t.IndexCode == indexCode);
                if (temp != null)
                {
                    StarIndexes.RemoveAt(i);
                }
            }

            for (int i = starIndexCodes.Count - 1; i >= 0; i--)
            {
                indexCode = starIndexCodes[i];
                var temp = checkInfos.FirstOrDefault(t => t.IndexCode == indexCode);
                if (temp != null)
                {
                    starIndexCodes.RemoveAt(i);
                }
            }

            if (SelectStarIndex == null) lastSelectStarIndex = SelectStarIndex = StarIndexes.First();

            SaveStarIndexes();
        }

        private void SaveStarIndexes()
        {
            ConfigHelper.StarIndexes = string.Join(",", starIndexCodes.OrderBy(t => t));
        }

        private void Copy()
        {
            var str = string.Join(",", IndexInfos.Where(t => StarIndexes.Any(x => x.Key == t.IndexCode)).SelectMany(t => t.FundBaseInfos.Select(f => f.FundId)));
            if (ClipboardHelper.SetText(str))
            {
                PublishStatusMessage("复制关注指数相关基金代码成功");
            }
            else
            {
                PublishStatusMessage("复制关注指数相关基金代码失败");
            }
        }

        private async void RefreshDetail()
        {
            if (TryGetCancellationTokenFault(out var token))
            {
                MessageBoxEx.ShowError("已有任务正在执行，请等任务执行完成，或取消任务");
                return;
            }

            PublishStatusMessage("开始刷新关注指数详情");
            var task = fundDataBase.GetFundBaseInfos(IndexInfos.Where(t => StarIndexes.Any(x => x.Key == t.IndexCode)), token, true);
            SetRunTask(task);
            var list = await task;
            IndexInfoRefresh();
            TaskCompleted();

            if (TaskIsCancel)
            {
                PublishStatusMessage("刷新关注指数详情任务已取消");
            }
            else
            {
                PublishStatusMessage("刷新关注指数详情成功");
            }
        }

        private void SelectChanged()
        {
            Filter();
        }

        private void SetItemsSource(bool isRefresh, IEnumerable<IndexInfo> infos)
        {
            IndexInfos.Clear();
            IndexInfos.AddRange(infos);

            InitFilterData(isRefresh);

            Filter();
        }

        private void Filter()
        {
            var keyWords = KeyWord.InputSplit();

            int order = 1;
            foreach (var item in IndexInfos)
            {
                item.IsShow = IsShow(item, keyWords);
                if (item.IsShow)
                {
                    item.OrderNumber = order++;
                }
            }
        }

        private bool IsShow(IndexInfo index, string[] keyWords)
        {
            if (keyWords.Length > 0 && keyWords.All(t => index.IndexCode.Contains(t) == false && index.IndexName.Contains(t) == false)) return false;
            if (SelectStarIndex != null && SelectStarIndex.Key.IsNotNullAndEmpty() && index.IndexCode != SelectStarIndex.Key) return false;
            return true;
        }

        private void InitFilterData(bool isRefresh)
        {
            var lastKey = isRefresh ? SelectStarIndex?.Key : "";
            InitStarIndexes();
            lastSelectStarIndex = SelectStarIndex = GetDefaultSelectItem(StarIndexes, lastKey);
        }

        private void InitStarIndexes()
        {
            StarIndexes.Clear();
            StarIndexes.Add(new FilterModel("", "全部"));

            foreach (var item in starIndexCodes)
            {
                var info = IndexInfos.FirstOrDefault(t => t.IndexCode == item);
                if (info != null) StarIndexes.Add(new FilterModel(info.IndexCode, info.IndexName));
            }
        }
    }
}
