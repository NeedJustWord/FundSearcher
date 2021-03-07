using Prism.Regions;

namespace FundSearcher.Views
{
    class FundCompareViewModel : BaseViewModel
    {
        public FundCompareViewModel(IRegionManager regionManager) : base(regionManager, RegionName.FundRegion)
        {
        }
    }
}
