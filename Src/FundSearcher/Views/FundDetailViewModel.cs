using Fund.Crawler.Models;
using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    internal class FundDetailViewModel : BaseFundViewModel
    {
        #region 属性
        private FundInfo fundInfo;
        /// <summary>
        /// 基金信息
        /// </summary>
        public FundInfo FundInfo
        {
            get { return fundInfo; }
            set { SetProperty(ref fundInfo, value); }
        }
        #endregion

        public FundDetailViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            if (navigationContext.Parameters.TryGetValue(ParameterName.DetailFundInfo, out FundInfo info))
            {
                FundInfo = info;
            }
            PublishStatusMessage($"基金[{FundInfo?.FundId},{FundInfo?.FundName}]详情数据加载完成");
        }
    }
}
