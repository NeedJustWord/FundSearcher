using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace FundSearcher.Extensions
{
    static class DependencyObjectExtension
    {
        /// <summary>
        /// 可视化树中查找指定类型的所有子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void FindVisualTreeChilds<T>(this DependencyObject obj, ref List<T> list) where T : FrameworkElement
        {
            var count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T t) list.Add(t);

                FindVisualTreeChilds(child, ref list);
            }
        }

        /// <summary>
        /// 可视化树中查找指定类型和名称的子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindVisualTreeChild<T>(this DependencyObject obj, string name) where T : FrameworkElement
        {
            var count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T t && t.Name == name) return t;

                var result = FindVisualTreeChild<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }

        /// <summary>
        /// 可视化树中查找指定类型的父对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualTreeParent<T>(this DependencyObject obj) where T : FrameworkElement
        {
            while (obj != null && !(obj is T))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }
            var result = obj as T;
            return result;
        }
    }
}
