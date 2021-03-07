using System;
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


        private ObservableCollection<FundInfo> fundInfos = new ObservableCollection<FundInfo>();
        public ObservableCollection<FundInfo> FundInfos
        {
            get { return fundInfos; }
            set { SetProperty(ref fundInfos, value); }
        }

        public FundQueryViewModel(IRegionManager regionManager, FundDataBase dataBase) : base(regionManager, RegionName.FundRegion)
        {
            fundDataBase = dataBase;
        }

        protected async override void Query()
        {
            var fundInfos = fundDataBase.GetFundInfos("007664,001592,110026,010183,003765");
            var list = (await fundInfos).OrderBy(t => t.FundId);
            var json = list.ToJson();
            SetItemsSource(list);
        }

        private void SetItemsSource(IEnumerable<FundInfo> infos)
        {
            FundInfos.Clear();
            FundInfos.AddRange(infos);
        }
    }
}
