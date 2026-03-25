using System.Linq;
using System.Windows;
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

        private Visibility applyRatesVisibility;
        /// <summary>
        /// 认购费率是否显示
        /// </summary>
        public Visibility ApplyRatesVisibility
        {
            get { return applyRatesVisibility; }
            set { SetProperty(ref applyRatesVisibility, value); }
        }

        private Visibility frontEndApplyRatesVisibility;
        /// <summary>
        /// 前端认购费率是否显示
        /// </summary>
        public Visibility FrontEndApplyRatesVisibility
        {
            get { return frontEndApplyRatesVisibility; }
            set { SetProperty(ref frontEndApplyRatesVisibility, value); }
        }

        private Visibility backEndApplyRatesVisibility;
        /// <summary>
        /// 后端认购费率是否显示
        /// </summary>
        public Visibility BackEndApplyRatesVisibility
        {
            get { return backEndApplyRatesVisibility; }
            set { SetProperty(ref backEndApplyRatesVisibility, value); }
        }

        private Visibility buyRatesVisibility;
        /// <summary>
        /// 申购费率是否显示
        /// </summary>
        public Visibility BuyRatesVisibility
        {
            get { return buyRatesVisibility; }
            set { SetProperty(ref buyRatesVisibility, value); }
        }

        private Visibility frontEndBuyRatesVisibility;
        /// <summary>
        /// 前端申购费率是否显示
        /// </summary>
        public Visibility FrontEndBuyRatesVisibility
        {
            get { return frontEndBuyRatesVisibility; }
            set { SetProperty(ref frontEndBuyRatesVisibility, value); }
        }

        private Visibility backEndBuyRatesVisibility;
        /// <summary>
        /// 后端申购费率是否显示
        /// </summary>
        public Visibility BackEndBuyRatesVisibility
        {
            get { return backEndBuyRatesVisibility; }
            set { SetProperty(ref backEndBuyRatesVisibility, value); }
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
                ApplyRatesVisibility = info.TransactionInfo.ApplyRates.Any(t => t.IsFront == null) ? Visibility.Visible : Visibility.Collapsed;
                FrontEndApplyRatesVisibility = info.TransactionInfo.ApplyRates.Any(t => t.IsFront == true) ? Visibility.Visible : Visibility.Collapsed;
                BackEndApplyRatesVisibility = info.TransactionInfo.ApplyRates.Any(t => t.IsFront == false) ? Visibility.Visible : Visibility.Collapsed;
                BuyRatesVisibility = info.TransactionInfo.BuyRates.Any(t => t.IsFront == null) ? Visibility.Visible : Visibility.Collapsed;
                FrontEndBuyRatesVisibility = info.TransactionInfo.BuyRates.Any(t => t.IsFront == true) ? Visibility.Visible : Visibility.Collapsed;
                BackEndBuyRatesVisibility = info.TransactionInfo.BuyRates.Any(t => t.IsFront == false) ? Visibility.Visible : Visibility.Collapsed;
            }
            PublishStatusMessage($"基金[{FundInfo?.FundId},{FundInfo?.FundName}]详情数据加载完成");
        }
    }
}
