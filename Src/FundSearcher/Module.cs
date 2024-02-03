using FundSearcher.Consts;
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
            regionManager.RequestNavigate(RegionName.Main, NavigateName.FundManager);
            regionManager.RequestNavigate(RegionName.Fund, NavigateName.FundQuery);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FundManager>(NavigateName.FundManager);
            containerRegistry.RegisterForNavigation<FundQuery>(NavigateName.FundQuery);
            containerRegistry.RegisterForNavigation<FundDetail>(NavigateName.FundDetail);
            containerRegistry.RegisterForNavigation<FundCompare>(NavigateName.FundCompare);
        }
    }
}
