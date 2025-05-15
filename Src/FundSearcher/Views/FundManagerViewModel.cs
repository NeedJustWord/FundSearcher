using FundSearcher.Consts;
using Prism.Regions;

namespace FundSearcher.Views
{
    class FundManagerViewModel : BaseFundViewModel
    {
        public FundManagerViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }

        protected override void OnFirstLoad()
        {
            Navigate(NavigateName.FundQuery);
        }
    }
}
