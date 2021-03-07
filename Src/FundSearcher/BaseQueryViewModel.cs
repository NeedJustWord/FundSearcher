using Prism.Commands;
using Prism.Regions;

namespace FundSearcher
{
    abstract class BaseQueryViewModel : BaseViewModel
    {
        public DelegateCommand QueryCommand { get; private set; }

        public BaseQueryViewModel(IRegionManager regionManager, string regionName) : base(regionManager, regionName)
        {
            QueryCommand = new DelegateCommand(Query);
        }

        protected abstract void Query();
    }
}
