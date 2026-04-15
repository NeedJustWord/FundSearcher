using System.Windows;

namespace FundSearcher.Attacheds
{
    internal class TextBoxAttached
    {
        #region 水印
        public static string GetWaterMark(DependencyObject obj)
        {
            return (string)obj.GetValue(WaterMarkProperty);
        }

        public static void SetWaterMark(DependencyObject obj, string value)
        {
            obj.SetValue(WaterMarkProperty, value);
        }

        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.RegisterAttached("WaterMark", typeof(string), typeof(TextBoxAttached), new PropertyMetadata(""));
        #endregion
    }
}
