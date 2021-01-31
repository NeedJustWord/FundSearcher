using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Crawler.DataHandler.Extensions
{
    /// <summary>
    /// <see cref="MatchCollection"/>扩展
    /// </summary>
    static class MatchCollectionExtension
    {
        /// <summary>
        /// 投影<see cref="MatchCollection"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(this MatchCollection collection, Func<Match, T> func)
        {
            if (collection != null && func != null)
            {
                foreach (Match match in collection)
                {
                    yield return func(match);
                }
            }
        }
    }
}
