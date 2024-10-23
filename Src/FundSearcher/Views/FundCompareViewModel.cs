using System.Collections.ObjectModel;
using System.Linq;
using Fund.Crawler.Models;
using FundSearcher.Consts;
using FundSearcher.Extensions;
using FundSearcher.Models;
using FundSearcher.PubSubEvents;
using Prism.Events;
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

        public FundCompareViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            eventAggregator.Subscribe<FundCompareFilterModelClickEvent, FilterModel>(FilterModelClick);
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

        private void FilterModelClick(FilterModel model)
        {
            model.IsSelected = !model.IsSelected;
            var info = FundInfos.First(t => t.FundId == model.Key);
            info.IsShow = model.IsSelected;
        }
    }
}
