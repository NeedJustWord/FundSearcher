using Fund.DataBase;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher.Views
{
    class IndexDetailViewModel : BaseIndexViewModel
    {
        public IndexDetailViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, eventAggregator, dataBase)
        {
        }

        protected override void OnFirstLoad()
        {
            PublishStatusMessage("指数详情数据加载完成");
        }
    }
}
