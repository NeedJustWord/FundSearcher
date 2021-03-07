using Prism.Regions;

namespace FundSearcher.Views
{
    class FundDetailViewModel : BaseViewModel
    {
        public FundDetailViewModel(IRegionManager regionManager) : base(regionManager, RegionName.FundRegion)
        {
        }
    }
}
