using System;
using System.Reflection;
using System.Xml;

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
        #endregion

        private static readonly XmlDocument document;
        private static readonly XmlNode appSettings;
        private static readonly string configPath;
        private static bool needSave;

        static ConfigHelper()
        {
            configPath = $"{Assembly.GetEntryAssembly().GetName().Name}.exe.config";
            document = new XmlDocument();
            document.Load(configPath);
            appSettings = GetOrCreateXmlNode(document, "configuration", "appSettings");

            starIndexes = Get(nameof(StarIndexes));
            blackFunds = Get(nameof(BlackFunds));
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
}
