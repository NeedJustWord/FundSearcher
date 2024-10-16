using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseFundViewModel : BaseViewModel
    {
        public BaseFundViewModel(IRegionManager regionManager, IEventAggregator eventAggregator = null) : base(regionManager, RegionName.Fund, eventAggregator)
        {
        }
    }
}
