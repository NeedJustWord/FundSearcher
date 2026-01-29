using Fund.DataBase;
using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseIndexViewModel : BaseTaskViewModel
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

        private bool keyWordIsFocused;
        /// <summary>
        /// 关键字是否具有焦点
        /// </summary>
        public bool KeyWordIsFocused
        {
            get { return keyWordIsFocused; }
            set { SetProperty(ref keyWordIsFocused, value); }
        }

        public BaseIndexViewModel(IRegionManager regionManager, IEventAggregator eventAggregator = null, FundDataBase fundDataBase = null) : base(regionManager, RegionName.Index, eventAggregator, fundDataBase)
        {
        }
    }
}
