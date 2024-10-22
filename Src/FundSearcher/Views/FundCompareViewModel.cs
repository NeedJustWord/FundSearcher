using System.Collections.ObjectModel;
using System.Linq;
using Fund.Crawler.Models;
using FundSearcher.Consts;
using FundSearcher.Models;
using Prism.Regions;

namespace FundSearcher.Views
{
    class FundCompareViewModel : BaseFundViewModel
    {
        #region 基金比较列表
        private ObservableCollection<FundInfo> fundInfos = new ObservableCollection<FundInfo>();
        public ObservableCollection<FundInfo> FundInfos
        {
            get { return fundInfos; }
            set { SetProperty(ref fundInfos, value); }
        }
        #endregion

        #region 显示基金
        private ObservableCollection<FilterModel> showFundInfos = new ObservableCollection<FilterModel>();
        /// <summary>
        /// 显示基金
        /// </summary>
        public ObservableCollection<FilterModel> ShowFundInfos
        {
            get { return showFundInfos; }
            set { SetProperty(ref showFundInfos, value); }
        }
        #endregion

        public FundCompareViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            FundInfos.Clear();
            ShowFundInfos.Clear();
            if (navigationContext.Parameters.TryGetValue(ParameterName.CompareFundInfos, out FundInfo[] infos))
            {
                FundInfos.AddRange(infos);
                ShowFundInfos.AddRange(infos.Select(t => new FilterModel(t.FundId, t.FundName, true)));
            }
        }
    }
}
