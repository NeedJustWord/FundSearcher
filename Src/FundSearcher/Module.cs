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
            regionManager.RequestNavigate(RegionName.Shell, NavigateName.FundManager);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FundManager>(NavigateName.FundManager);
            containerRegistry.RegisterForNavigation<FundQuery>(NavigateName.FundQuery);
            containerRegistry.RegisterForNavigation<FundCompare>(NavigateName.FundCompare);
            containerRegistry.RegisterForNavigation<FundBlack>(NavigateName.FundBlack);
            containerRegistry.RegisterForNavigation<FundDetail>(NavigateName.FundDetail);

            containerRegistry.RegisterForNavigation<IndexManager>(NavigateName.IndexManager);
            containerRegistry.RegisterForNavigation<IndexQuery>(NavigateName.IndexQuery);
            containerRegistry.RegisterForNavigation<IndexDetail>(NavigateName.IndexDetail);
        }
    }
}
