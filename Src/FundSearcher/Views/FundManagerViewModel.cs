using Prism.Regions;

namespace FundSearcher.Views
{
    class FundManagerViewModel : BaseViewModel
    {
        public FundManagerViewModel(IRegionManager regionManager) : base(regionManager, RegionName.FundRegion)
        {
        }
    }
}
