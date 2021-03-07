using Prism.Commands;
using Prism.Regions;

namespace FundSearcher
{
    class BaseWindowViewModel : BaseViewModel
    {
        public DelegateCommand ClosingCommand { get; private set; }

        public BaseWindowViewModel(IRegionManager regionManager, string regionName) : base(regionManager, regionName)
        {
            ClosingCommand = new DelegateCommand(OnClosing);
        }

        protected virtual void OnClosing()
        {
        }
    }
}
