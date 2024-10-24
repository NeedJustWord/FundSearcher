using System;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Fund.Crawler.Models
{
    public abstract class BaseInfo : BindableBase
    {
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
