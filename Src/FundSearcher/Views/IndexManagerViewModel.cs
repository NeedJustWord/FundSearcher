using FundSearcher.Consts;
using Prism.Regions;

namespace FundSearcher.Views
{
    class IndexManagerViewModel : BaseIndexViewModel
    {
        public IndexManagerViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }

        protected override void OnFirstLoad()
        {
            Navigate(NavigateName.IndexQuery);
        }
    }
}
