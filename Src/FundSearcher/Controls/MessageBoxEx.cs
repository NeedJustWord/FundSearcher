using System.Windows;

namespace FundSearcher.Controls
{
    static class MessageBoxEx
    {
        public static void ShowError(string msg)
        {
            Show(msg, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static MessageBoxResult Show(string msg, MessageBoxButton button, MessageBoxImage image)
        {
            return MessageBox.Show(msg, "基金检索工具", button, image);
        }
    }
}
