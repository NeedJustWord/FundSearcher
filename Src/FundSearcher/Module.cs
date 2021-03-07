using FundSearcher.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace FundSearcher
{
    class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate(RegionName.MainRegion, RegionName.FundManagerNavigate);
            regionManager.RequestNavigate(RegionName.FundRegion, RegionName.FundQueryNavigate);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FundManager>(RegionName.FundManagerNavigate);
            containerRegistry.RegisterForNavigation<FundQuery>(RegionName.FundQueryNavigate);
            containerRegistry.RegisterForNavigation<FundDetail>(RegionName.FundDetailNavigate);
            containerRegistry.RegisterForNavigation<FundCompare>(RegionName.FundCompareNavigate);
        }
    }
}
