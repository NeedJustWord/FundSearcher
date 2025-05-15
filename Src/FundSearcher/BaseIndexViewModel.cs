using Fund.DataBase;
using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseIndexViewModel : BaseViewModel
    {
        public BaseIndexViewModel(IRegionManager regionManager, IEventAggregator eventAggregator = null, FundDataBase fundDataBase = null) : base(regionManager, RegionName.Index, eventAggregator, fundDataBase)
        {
        }
    }
}
