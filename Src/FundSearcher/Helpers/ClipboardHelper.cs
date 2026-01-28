using System;
using System.Runtime.InteropServices;

namespace FundSearcher.Helpers
{
    /// <summary>
    /// 剪贴板帮助类
    /// </summary>
    static class ClipboardHelper
    {
        /// <summary>
        /// 复制文本到剪贴板，返回是否成功
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool SetText(string text)
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                return false;
            }

            try
            {
                if (!EmptyClipboard())
                {
                    return false;
                }

                var ptr = Marshal.StringToHGlobalUni(text);
                if (SetClipboardData(CF_UNICODETEXT, ptr) == IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                CloseClipboard();
            }
        }

        #region 剪贴板API https://learn.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-openclipboard
        /// <summary>
        /// 打开剪贴板
        /// </summary>
        /// <param name="hWndNewOwner">要与打开的剪贴板关联的窗口的句柄。 如果此参数为 NULL，则打开的剪贴板与当前任务相关联</param>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// 关闭剪贴板
        /// </summary>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern bool CloseClipboard();

        /// <summary>
        /// 清空剪贴板并释放剪贴板中数据的句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern bool EmptyClipboard();

        /// <summary>
        /// 确定剪贴板是否包含指定格式的数据
        /// </summary>
        /// <param name="format">标准或已注册的剪贴板格式</param>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern bool IsClipboardFormatAvailable(uint format);

        /// <summary>
        /// 从剪贴板中检索指定格式的数据
        /// </summary>
        /// <param name="format">剪贴板格式</param>
        /// <returns>
        /// 如果函数成功，则返回值是指定格式的剪贴板对象的句柄。
        /// 如果函数失败，则返回值为 NULL。 要获得更多的错误信息，请调用 GetLastError。
        /// </returns>
        [DllImport("User32")]
        public static extern IntPtr GetClipboardData(uint format);

        /// <summary>
        /// 将数据以指定的剪贴板格式放置在剪贴板上
        /// </summary>
        /// <param name="format">剪贴板格式</param>
        /// <param name="hMem">指定格式的数据的句柄</param>
        /// <returns>
        /// 如果函数成功，则返回值是数据的句柄。
        /// 如果函数失败，则返回值为 NULL。 要获得更多的错误信息，请调用 GetLastError。
        /// </returns>
        [DllImport("User32", CharSet = CharSet.Unicode)]
        public static extern IntPtr SetClipboardData(uint format, IntPtr hMem);
        #endregion

        #region 剪贴板格式 https://learn.microsoft.com/zh-cn/windows/win32/dataxchg/standard-clipboard-formats
        /// <summary>
        /// Unicode文本格式
        /// </summary>
        private const uint CF_UNICODETEXT = 13;
        #endregion
    }
}
