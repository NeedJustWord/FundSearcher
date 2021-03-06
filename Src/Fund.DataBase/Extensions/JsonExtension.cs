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
        /// <returns></returns>
        public static string ToJson<T>(this T t)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
            };
            return JsonConvert.SerializeObject(t, Formatting.Indented, settings);
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
