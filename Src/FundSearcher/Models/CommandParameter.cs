using System;

namespace FundSearcher.Models
{
    /// <summary>
    /// 命令参数
    /// </summary>
    internal class CommandParameter
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object[] Params { get; set; }

        public CommandParameter(object[] objs)
        {
            CommandName = objs[0].ToString();
            var length = objs.Length - 1;
            Params = new object[length];
            if (length > 0) Array.Copy(objs, 1, Params, 0, length);
        }
    }
}
