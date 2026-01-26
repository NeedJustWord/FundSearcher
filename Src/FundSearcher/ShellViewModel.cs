using System;
using System.Collections.Generic;
using System.Linq;
using Fund.Core.Helpers;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Fund.Crawler.PubSubEvents;
using Fund.DataBase;
using FundSearcher.Consts;
using FundSearcher.Helpers;
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

        private string errorMessage;
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }
        #endregion

        private List<CrawlingProgressModel> errorModels = new List<CrawlingProgressModel>();

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
            ConfigHelper.Save();
        }

        private void HandleCrawleProgress(CrawlingProgressModel model)
        {
            Total = model.Total;
            Current = model.Current;
            Message = model.Message;

            if (model.Exception != null)
            {
                ErrorMessage = "爬取过程中出现异常";
                errorModels.Add(model);
            }

            if (ErrorMessage != null && model.Current == 0)
            {
                ErrorMessage = null;
                errorModels.Clear();
            }
            else if (model.Finish && errorModels.Count > 0)
            {
                var str = string.Join($"{Environment.NewLine}{Environment.NewLine}", errorModels.Select(t => $"爬取消息：{t.Message}{Environment.NewLine}异常消息：{t.Exception.Message}{Environment.NewLine}异常堆栈：{Environment.NewLine}{t.Exception.StackTrace}"));
                if (ClipboardHelper.SetText(str))
                {
                    ErrorMessage = "爬取过程中出现异常，异常信息已复制到剪贴板";
                }
                else
                {
                    ErrorMessage = "爬取过程中出现异常，但异常信息复制到剪贴板失败";
                }
            }
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
