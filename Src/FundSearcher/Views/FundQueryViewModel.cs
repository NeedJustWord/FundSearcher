using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fund.Crawler.Models;
using Fund.DataBase;
using Prism.Regions;

namespace FundSearcher.Views
{
    class FundQueryViewModel : BaseQueryViewModel
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

        public FundQueryViewModel(IRegionManager regionManager, FundDataBase dataBase) : base(regionManager, RegionName.FundRegion)
        {
            fundDataBase = dataBase;
        }

        protected async override void Query()
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

        private void SetItemsSource(IEnumerable<FundInfo> infos)
        {
            FundInfos.Clear();
            FundInfos.AddRange(infos);
        }
    }
}
