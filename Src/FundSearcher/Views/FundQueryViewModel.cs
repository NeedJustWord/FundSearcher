using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fund.Crawler.Models;
using Fund.DataBase;
using FundSearcher.Consts;
using Prism.Regions;

namespace FundSearcher.Views
{
    class FundQueryViewModel : BaseFundViewModel
    {
        private readonly FundDataBase fundDataBase;

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
        #endregion

        public FundQueryViewModel(IRegionManager regionManager, FundDataBase dataBase) : base(regionManager)
        {
            fundDataBase = dataBase;
            RegisterCommand(CommandName.Query, Query);
            RegisterCommand(CommandName.Refresh, Refresh);
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

            var data = list.OrderBy(t => t.FundId).ToList();
            int order = 1;
            foreach (var item in data)
            {
                item.OrderNumber = order++;
            }
            SetItemsSource(data);
        }

        private async void Refresh()
        {
            var fundIds = fundInfos.Where(t => t.IsChecked).Select(t => t.FundId).ToArray();
            if (fundIds.Length == 0)
            {
                MessageBox.Show("请勾选需要刷新的基金", "基金检索工具", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<FundInfo> list = await fundDataBase.GetFundInfos(true, fundIds);
            foreach (var item in list)
            {
                for (int i = 0; i < FundInfos.Count; i++)
                {
                    if (FundInfos[i].FundId == item.FundId) FundInfos[i] = item;
                }
            }
        }

        private void SetItemsSource(IEnumerable<FundInfo> infos)
        {
            FundInfos.Clear();
            FundInfos.AddRange(infos);
        }
    }
}
