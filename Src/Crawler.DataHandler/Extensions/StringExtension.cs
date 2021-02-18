using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Crawler.DataHandler.Extensions
{
    /// <summary>
    /// <see cref="string"/>扩展
    /// </summary>
    public static class StringExtension
    {
        #region 获取指定标签
        /// <summary>
        /// 获取指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static MatchCollection GetHtmlTag(this string input, string htmlTag)
        {
            return HtmlDecode.GetHtmlTag(input, htmlTag);
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Match GetHtmlTag(this string input, string htmlTag, int index)
        {
            return HtmlDecode.GetHtmlTag(input, htmlTag, index);
        }

        /// <summary>
        /// 获取指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetHtmlTagValue(this string input, string htmlTag)
        {
            return HtmlDecode.GetHtmlTagValue(input, htmlTag);
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetHtmlTagValue(this string input, string htmlTag, int index)
        {
            return HtmlDecode.GetHtmlTagValue(input, htmlTag, index);
        }

        /// <summary>
        /// 获取第一个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static Match GetFirstHtmlTag(this string input, string htmlTag)
        {
            return HtmlDecode.GetFirstHtmlTag(input, htmlTag);
        }

        /// <summary>
        /// 获取第一个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static string GetFirstHtmlTagValue(string input, string htmlTag)
        {
            return HtmlDecode.GetFirstHtmlTagValue(input, htmlTag);
        }
        #endregion

        #region 获取具有指定属性键值对的指定标签
        /// <summary>
        /// 获取具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static MatchCollection GetHtmlTagByAttri(this string input, string htmlTag, string attriName, string attriValue)
        {
            return HtmlDecode.GetHtmlTagByAttri(input, htmlTag, attriName, attriValue);
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Match GetHtmlTagByAttri(this string input, string htmlTag, string attriName, string attriValue, int index)
        {
            return HtmlDecode.GetHtmlTagByAttri(input, htmlTag, attriName, attriValue, index);
        }

        /// <summary>
        /// 获取具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetHtmlTagValueByAttri(this string input, string htmlTag, string attriName, string attriValue)
        {
            return HtmlDecode.GetHtmlTagValueByAttri(input, htmlTag, attriName, attriValue);
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetHtmlTagValueByAttri(this string input, string htmlTag, string attriName, string attriValue, int index)
        {
            return HtmlDecode.GetHtmlTagValueByAttri(input, htmlTag, attriName, attriValue, index);
        }

        /// <summary>
        /// 获取第一个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static Match GetFirstHtmlTagByAttri(string input, string htmlTag, string attriName, string attriValue)
        {
            return HtmlDecode.GetFirstHtmlTagByAttri(input, htmlTag, attriName, attriValue);
        }

        /// <summary>
        /// 获取第一个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static string GetFirstHtmlTagValueByAttri(string input, string htmlTag, string attriName, string attriValue)
        {
            return HtmlDecode.GetFirstHtmlTagValueByAttri(input, htmlTag, attriName, attriValue);
        }
        #endregion

        /// <summary>
        /// 获取标签的内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHtmlTagContent(this string input)
        {
            return HtmlDecode.GetHtmlTagContent(input);
        }
    }
}
