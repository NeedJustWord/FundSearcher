using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Fund.Crawler.PubSubEvents;
using Fund.DataBase;
using FundSearcher.Consts;
using FundSearcher.PubSubEvents;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class ShellViewModel : BaseWindowViewModel
    {
        private string title = "基金检索工具";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #region 状态栏信息
        private int total;
        /// <summary>
        /// 爬取总数
        /// </summary>
        public int Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        private int current;
        /// <summary>
        /// 爬取当前进度
        /// </summary>
        public int Current
        {
            get { return current; }
            set { SetProperty(ref current, value); }
        }

        private string message;
        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
        #endregion

        public ShellViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, FundDataBase dataBase) : base(regionManager, RegionName.Shell, eventAggregator, dataBase)
        {
            eventAggregator.Subscribe<CrawlingProgressEvent, CrawlingProgressModel>(HandleCrawleProgress);
            eventAggregator.Subscribe<StatusMessageEvent, string>(HandleStatusMessage);
            RegisterCommand(CommandName.FundMenu, NavigateToFund);
            RegisterCommand(CommandName.IndexMenu, NavigateToIndex);
        }

        protected override void OnClosing()
        {
            fundDataBase.Save();
        }

        private void HandleCrawleProgress(CrawlingProgressModel model)
        {
            Total = model.Total;
            Current = model.Current;
            Message = model.Message;
        }

        private void HandleStatusMessage(string msg)
        {
            Message = msg;
        }

        private void NavigateToFund()
        {
            Navigate(NavigateName.FundManager);
        }

        private void NavigateToIndex()
        {
            Navigate(NavigateName.IndexManager);
        }
    }
}
