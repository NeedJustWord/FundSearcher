using Newtonsoft.Json;

namespace System
{
    /// <summary>
    /// Json扩展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 序列化成Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ignoreNull">是否忽略null</param>
        /// <param name="isIndented">是否格式化</param>
        /// <returns></returns>
        public static string ToJson<T>(this T t, bool ignoreNull = false, bool isIndented = true)
        {
            var formatting = isIndented ? Formatting.Indented : Formatting.None;
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include,
            };
            return JsonConvert.SerializeObject(t, formatting, settings);
        }

        /// <summary>
        /// 从Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
