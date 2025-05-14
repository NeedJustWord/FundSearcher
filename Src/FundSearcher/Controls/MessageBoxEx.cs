using System.Windows;

namespace FundSearcher.Controls
{
    static class MessageBoxEx
    {
        public static void ShowError(string msg)
        {
            Show(msg, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowQuestion(string msg)
        {
            return Show(msg, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        private static MessageBoxResult Show(string msg, MessageBoxButton button, MessageBoxImage image)
        {
            return MessageBox.Show(msg, "基金检索工具", button, image);
        }
    }
}
