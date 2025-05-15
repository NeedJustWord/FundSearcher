using Fund.DataBase;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseWindowViewModel : BaseViewModel
    {
        public DelegateCommand ClosingCommand { get; private set; }

        public BaseWindowViewModel(IRegionManager regionManager, string regionName, IEventAggregator eventAggregator, FundDataBase fundDataBase) : base(regionManager, regionName, eventAggregator, fundDataBase)
        {
            ClosingCommand = new DelegateCommand(OnClosing);
        }

        protected virtual void OnClosing()
        {
        }
    }
}
