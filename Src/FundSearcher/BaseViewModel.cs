using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace FundSearcher
{
    class BaseViewModel : BindableBase, INavigationAware
    {
        private readonly Dictionary<string, Action<object>> dictDelegateCommand;

        protected readonly IEventAggregator eventAggregator;
        protected readonly IRegionManager regionManager;
        protected readonly string regionName;
        protected IRegionNavigationJournal journal;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand<object> DictCommand { get; private set; }

        public BaseViewModel(IRegionManager regionManager, string regionName, IEventAggregator eventAggregator = null)
        {
            dictDelegateCommand = new Dictionary<string, Action<object>>();

            this.regionManager = regionManager;
            this.regionName = regionName;
            this.eventAggregator = eventAggregator;

            LoadedCommand = new DelegateCommand(OnLoaded);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            DictCommand = new DelegateCommand<object>(Dict);
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

        private void Dict(object obj)
        {
            if (obj is string commandName)
            {
                if (dictDelegateCommand.TryGetValue(commandName, out var action))
                {
                    action(null);
                }
                return;
            }
        }

        public void RegisterCommand(string commandName, Action action)
        {
            RegisterCommand(commandName, o => action());
        }

        public void RegisterCommand(string commandName, Action<object> action)
        {
            dictDelegateCommand[commandName] = action;
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
