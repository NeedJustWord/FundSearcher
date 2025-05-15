using Fund.DataBase;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class IndexQueryViewModel : BaseIndexViewModel
    {
        public IndexQueryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, eventAggregator, dataBase)
        {
        }

        protected override void OnFirstLoad()
        {
            PublishStatusMessage("指数数据加载完成");
        }
    }
}
