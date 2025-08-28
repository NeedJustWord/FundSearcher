using Fund.DataBase;
using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseFundViewModel : BaseTaskViewModel
    {
        public BaseFundViewModel(IRegionManager regionManager, IEventAggregator eventAggregator = null, FundDataBase fundDataBase = null) : base(regionManager, RegionName.Fund, eventAggregator, fundDataBase)
        {
        }
    }
}
