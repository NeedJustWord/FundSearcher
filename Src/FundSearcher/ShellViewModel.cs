using Fund.DataBase;
using Prism.Regions;

namespace FundSearcher
{
    class ShellViewModel : BaseWindowViewModel
    {
        private readonly FundDataBase fundDataBase;

        private string title = "基金检索工具";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ShellViewModel(IRegionManager regionManager, FundDataBase dataBase) : base(regionManager, RegionName.MainRegion)
        {
            fundDataBase = dataBase;
        }

        protected override void OnClosing()
        {
            fundDataBase.Save();
        }
    }
}
