using System;
using System.Xml;
using Fund.Core.Extensions;

namespace Fund.Core.Helpers
{
    public static class ConfigHelper
    {
        #region 属性
        private static string starIndexes;
        /// <summary>
        /// 关注指数
        /// </summary>
        public static string StarIndexes
        {
            get { return starIndexes; }
            set
            {
                if (starIndexes != value)
                {
                    starIndexes = value;
                    needSave = true;
                }
            }
        }

        private static string blackFunds;
        /// <summary>
        /// 黑名单基金
        /// </summary>
        public static string BlackFunds
        {
            get { return blackFunds; }
            set
            {
                if (blackFunds != value)
                {
                    blackFunds = value;
                    needSave = true;
                }
            }
        }

        private static string holdingFunds;
        /// <summary>
        /// 持仓基金
        /// </summary>
        public static string HoldingFunds
        {
            get { return holdingFunds; }
            set
            {
                if (holdingFunds != value)
                {
                    holdingFunds = value;
                    needSave = true;
                }
            }
        }

        private static bool cachePageSource;
        /// <summary>
        /// 是否缓存页面源代码
        /// </summary>
        public static bool CachePageSource
        {
            get { return cachePageSource; }
            set
            {
                if (cachePageSource != value)
                {
                    cachePageSource = value;
                    needSave = true;
                }
            }
        }

        private static byte cacheOverDay;
        /// <summary>
        /// 页面源代码缓存过期天数
        /// </summary>
        public static byte CacheOverDay
        {
            get { return cacheOverDay; }
            set
            {
                if (cacheOverDay != value)
                {
                    cacheOverDay = value;
                    needSave = true;
                }
            }
        }

        private static byte fundOverDay;
        /// <summary>
        /// 基金信息过期天数
        /// </summary>
        public static byte FundOverDay
        {
            get { return fundOverDay; }
            set
            {
                if (fundOverDay != value)
                {
                    fundOverDay = value;
                    needSave = true;
                }
            }
        }

        /// <summary>
        /// 缓存目录
        /// </summary>
        public static string CachePath => "Cache";

        /// <summary>
        /// 单位净值更新时间
        /// </summary>
        public static TimeSpan PriceUpdateTime => new TimeSpan(20, 0, 0);
        #endregion

        private static readonly XmlDocument document;
        private static readonly XmlNode appSettings;
        private static readonly string configPath;
        private static bool needSave;

        static ConfigHelper()
        {
            configPath = Config.ConfigPath;
            document = new XmlDocument();
            document.Load(configPath);
            appSettings = GetOrCreateXmlNode(document, "configuration", "appSettings");

            starIndexes = Get(nameof(StarIndexes));
            blackFunds = Get(nameof(BlackFunds));
            holdingFunds = Get(nameof(HoldingFunds));
            cachePageSource = Get(nameof(CachePageSource)).AsBool();
            cacheOverDay = Get(nameof(CacheOverDay)).AsByte(0);
            fundOverDay = Get(nameof(FundOverDay)).AsByte(7);
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public static void Save()
        {
            if (needSave)
            {
                Set(nameof(StarIndexes), StarIndexes);
                Set(nameof(BlackFunds), BlackFunds);
                Set(nameof(HoldingFunds), HoldingFunds);
                Set(nameof(CachePageSource), CachePageSource);
                Set(nameof(CacheOverDay), CacheOverDay);
                Set(nameof(FundOverDay), FundOverDay);
                document.Save(configPath);
            }
        }

        private static XmlNode GetXmlNode(string key)
        {
            return appSettings.SelectSingleNode($"add[@key='{key}']");
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string Get(string key)
        {
            var node = GetXmlNode(key);
            return node == null ? "" : node.Attributes["value"].Value;
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool Set(string key, bool value)
        {
            return Set(key, value ? "1" : "0");
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool Set(string key, byte value)
        {
            return Set(key, value.ToString());
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool Set(string key, string value)
        {
            try
            {
                var node = GetXmlNode(key);
                if (node == null)
                {
                    var element = document.CreateElement("add");
                    element.SetAttribute("key", key);
                    element.SetAttribute("value", value);
                    appSettings.AppendChild(element);
                }
                else
                {
                    node.Attributes["value"].Value = value;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static XmlNode GetOrCreateXmlNode(XmlDocument root, params string[] names)
        {
            var xpath = $"/{string.Join("/", names)}";
            var node = root.SelectSingleNode(xpath);
            if (node != null) return node;

            XmlNode findNode = root;
            for (int i = 0; i < names.Length; i++)
            {
                var notFind = true;
                var name = names[i];
                foreach (XmlNode item in findNode.ChildNodes)
                {
                    if (item.Name == name)
                    {
                        notFind = false;
                        findNode = item;
                        break;
                    }
                }

                if (notFind)
                {
                    var create = document.CreateElement(name);
                    findNode.AppendChild(create);
                    findNode = create;
                }
            }
            return findNode;
        }
    }

    public static class Config
    {
        public static string ConfigPath;
    }
}
