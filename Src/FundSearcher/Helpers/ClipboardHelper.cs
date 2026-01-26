using System;
using System.Runtime.InteropServices;

namespace FundSearcher.Helpers
{
    /// <summary>
    /// 剪贴板帮助类
    /// <para>https://learn.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-openclipboard</para>
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
            EmptyClipboard();

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.StringToHGlobalUni(text);
                SetClipboardData(Format.CF_UNICODETEXT, ptr);
                CloseClipboard();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (ptr != IntPtr.Zero) Marshal.FreeHGlobal(ptr);
            }
        }

        #region Windows API
        /// <summary>
        /// 打开剪贴板
        /// </summary>
        /// <param name="hWndNewOwner"></param>
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
        /// <param name="format"></param>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern bool IsClipboardFormatAvailable(uint format);

        /// <summary>
        /// 从剪贴板中检索指定格式的数据
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern IntPtr GetClipboardData(uint format);

        /// <summary>
        /// 将数据以指定的剪贴板格式放置在剪贴板上
        /// </summary>
        /// <param name="format"></param>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("User32", CharSet = CharSet.Unicode)]
        public static extern IntPtr SetClipboardData(uint format, IntPtr hMem);

        /// <summary>
        /// 剪贴板格式
        /// <para>https://learn.microsoft.com/zh-cn/windows/win32/dataxchg/standard-clipboard-formats</para>
        /// </summary>
        class Format
        {
            /// <summary>
            /// Unicode 文本格式
            /// </summary>
            public const uint CF_UNICODETEXT = 13;
        }
        #endregion
    }
}
