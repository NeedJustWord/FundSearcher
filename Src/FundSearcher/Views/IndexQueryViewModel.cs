using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fund.Crawler.Models;
using Fund.DataBase;
using FundSearcher.Consts;
using FundSearcher.Controls;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class IndexQueryViewModel : BaseIndexViewModel
    {
        #region 属性
        private string keyWord;
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord
        {
            get { return keyWord; }
            set { SetProperty(ref keyWord, value); }
        }

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
        #endregion

        public IndexQueryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, eventAggregator, dataBase)
        {
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Reset, Reset);
            RegisterCommand(CommandName.Refresh, Refresh);
            RegisterCommand(CommandName.Detail, Detail);
        }

        protected override void OnFirstLoad()
        {
            Query();
            PublishStatusMessage("指数数据加载完成");
        }

        private async void Query()
        {
            var list = await fundDataBase.GetIndexInfos();
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
            var list = await fundDataBase.GetIndexInfos(true);
            SetItemsSource(list.CustomSort());
            Filter();
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

        private void SetItemsSource(IEnumerable<IndexInfo> infos)
        {
            IndexInfos.Clear();
            IndexInfos.AddRange(infos);
        }

        private void Filter()
        {
            var keyWords = keyWord == null ? new string[0] : keyWord.Split(FundDataBase.InputSeparator, StringSplitOptions.RemoveEmptyEntries);

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
            if (keyWords.Length == 0) return true;
            return keyWords.Any(t => index.IndexCode.Contains(t) || index.IndexName.Contains(t));
        }
    }
}
