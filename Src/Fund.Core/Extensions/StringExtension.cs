namespace Fund.Core.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullAndEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static double AsDouble(this string str)
        {
            return str.EndsWith("%") ? double.Parse(str.Substring(0, str.Length - 1)) / 100 : double.Parse(str);
        }
    }
}
