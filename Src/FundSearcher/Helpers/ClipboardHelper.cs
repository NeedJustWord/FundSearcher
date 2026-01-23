using System.Threading;
using System.Windows;

namespace FundSearcher.Helpers
{
    /// <summary>
    /// 剪贴板帮助类
    /// </summary>
    static class ClipboardHelper
    {
        /// <summary>
        /// 复制文本到剪贴板
        /// </summary>
        /// <param name="text"></param>
        public static void SetText(string text)
        {
            Thread thread = new Thread(() =>
            {
                Clipboard.SetText(text);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
