using Fund.DataBase;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseFundWithKeyWordViewModel : BaseFundViewModel
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

        public BaseFundWithKeyWordViewModel(IRegionManager regionManager, IEventAggregator eventAggregator = null, FundDataBase fundDataBase = null) : base(regionManager, eventAggregator, fundDataBase)
        {
        }
    }
}
