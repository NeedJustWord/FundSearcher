using System;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Fund.Crawler.Models
{
    public abstract class BaseInfo : BindableBase
    {
        private bool isShow;
        /// <summary>
        /// 是否显示
        /// </summary>
        [JsonIgnore]
        public bool IsShow
        {
            get { return isShow; }
            set { SetProperty(ref isShow, value); }
        }

        private int orderNumber;
        /// <summary>
        /// 序号
        /// </summary>
        [JsonIgnore]
        public int OrderNumber
        {
            get { return orderNumber; }
            set { SetProperty(ref orderNumber, value); }
        }

        private DateTime updateTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty(Order = -9)]
        public DateTime UpdateTime
        {
            get { return updateTime; }
            set { SetProperty(ref updateTime, value); }
        }

        private string infoSource;
        /// <summary>
        /// 信息来源
        /// </summary>
        [JsonProperty(Order = -8)]
        public string InfoSource
        {
            get { return infoSource; }
            set { SetProperty(ref infoSource, value); }
        }
    }
}
