using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fund.Core.Extensions;
using Fund.Crawler.Extensions;
using Fund.DataBase;
using FundSearcher.Models;
using FundSearcher.PubSubEvents;
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
        protected readonly FundDataBase fundDataBase;
        protected readonly string regionName;
        protected IRegionNavigationJournal journal;
        private bool isFirstLoad = true;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand<object> DictCommand { get; private set; }

        public BaseViewModel(IRegionManager regionManager, string regionName, IEventAggregator eventAggregator = null, FundDataBase fundDataBase = null)
        {
            dictDelegateCommand = new Dictionary<string, Action<object>>();

            this.regionManager = regionManager;
            this.regionName = regionName;
            this.eventAggregator = eventAggregator;
            this.fundDataBase = fundDataBase;

            LoadedCommand = new DelegateCommand(InternalOnLoaded);
            GoBackCommand = new DelegateCommand(GoBack);
            DictCommand = new DelegateCommand<object>(Dict);
        }

        private void InternalOnLoaded()
        {
            if (isFirstLoad)
            {
                isFirstLoad = false;
                OnFirstLoad();
            }
            OnLoaded();
        }

        /// <summary>
        /// 第一次加载执行
        /// </summary>
        protected virtual void OnFirstLoad()
        {
        }

        /// <summary>
        /// 每次加载执行
        /// </summary>
        protected virtual void OnLoaded()
        {
        }

        protected void Navigate(string navigatePath)
        {
            if (navigatePath != null) regionManager.RequestNavigate(regionName, navigatePath);
        }

        protected void Navigate(string navigatePath, NavigationParameters navigationParameters)
        {
            if (navigatePath != null) regionManager.RequestNavigate(regionName, navigatePath, navigationParameters);
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

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            journal = navigationContext.NavigationService.Journal;
        }

        protected void PublishStatusMessage(string msg)
        {
            eventAggregator.Publish<StatusMessageEvent, string>(msg);
        }

        protected void SetLastUnselected(FilterModel last, FilterModel value)
        {
            if (last != null && last != value) last.IsSelected = false;
        }

        protected void SetValueSelected(FilterModel value)
        {
            if (value != null) value.IsSelected = true;
        }

        protected FilterModel GetDefaultSelectItem(ObservableCollection<FilterModel> data, string key)
        {
            var item = key.IsNotNullAndEmpty() ? (data.FirstOrDefault(t => t.Key == key) ?? data.First()) : data.First();
            item.IsSelected = true;
            return item;
        }
    }
}
