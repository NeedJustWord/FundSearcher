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

        public static double AsDouble(this string str, double nullAsValue)
        {
            if (str.EndsWith("%"))
            {
                return double.Parse(str.Substring(0, str.Length - 1)) / 100;
            }

            return double.TryParse(str, out var value) ? value : nullAsValue;
        }

        public static double? AsDoubleNullable(this string str)
        {
            if (str.EndsWith("%"))
            {
                return double.Parse(str.Substring(0, str.Length - 1)) / 100;
            }

            return double.TryParse(str, out var value) ? value : (double?)null;
        }
    }
}
