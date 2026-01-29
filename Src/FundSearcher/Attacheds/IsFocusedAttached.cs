using System.Windows;
using System.Windows.Input;

namespace FundSearcher.Attacheds
{
    /// <summary>
    /// 是否具有焦点附加属性
    /// </summary>
    public static class IsFocusedAttached
    {
        #region 是否具有焦点
        public static bool? GetIsFocused(DependencyObject obj)
        {
            return (bool?)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool? value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool?), typeof(IsFocusedAttached), new FrameworkPropertyMetadata(IsFocusedChanged)
            {
                BindsTwoWayByDefault = true
            });

        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.GotFocus -= FrameworkElement_GotFocus;
                fe.LostFocus -= FrameworkElement_LostFocus;
                fe.GotFocus += FrameworkElement_GotFocus;
                fe.LostFocus += FrameworkElement_LostFocus;
            }

            if ((bool)e.NewValue)
            {
                if (fe.IsFocused == false) fe.Focus();
            }
            else
            {
                if (fe.IsFocused) fe.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, true);
        }

        private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, false);
        }
        #endregion
    }
}
