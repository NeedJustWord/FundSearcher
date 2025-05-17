using Fund.DataBase;
using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseIndexViewModel : BaseViewModel
    {
        private string keyWord;
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord
        {
            get { return keyWord; }
            set { SetProperty(ref keyWord, value); }
        }

        public BaseIndexViewModel(IRegionManager regionManager, IEventAggregator eventAggregator = null, FundDataBase fundDataBase = null) : base(regionManager, RegionName.Index, eventAggregator, fundDataBase)
        {
        }
    }
}
