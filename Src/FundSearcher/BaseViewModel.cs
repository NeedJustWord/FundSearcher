using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace FundSearcher
{
    class BaseViewModel : BindableBase, INavigationAware
    {
        protected readonly IRegionManager regionManager;
        protected readonly string regionName;
        protected IRegionNavigationJournal journal;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }

        public BaseViewModel(IRegionManager regionManager, string regionName)
        {
            this.regionManager = regionManager;
            this.regionName = regionName;

            LoadedCommand = new DelegateCommand(OnLoaded);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
        }

        protected virtual void OnLoaded()
        {
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null) regionManager.RequestNavigate(regionName, navigatePath);
        }

        private void GoBack()
        {
            journal.GoBack();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            journal = navigationContext.NavigationService.Journal;
        }
    }
}
