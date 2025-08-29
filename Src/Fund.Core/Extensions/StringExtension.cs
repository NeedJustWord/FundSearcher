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
            if (str == null) return nullAsValue;

            double value = 0;
            if (str.TryParseDouble(ref value))
            {
                return value;
            }

            return double.TryParse(str, out value) ? value : nullAsValue;
        }

        public static double? AsDoubleNullable(this string str)
        {
            if (str == null) return null;

            double value = 0;
            if (str.TryParseDouble(ref value))
            {
                return value;
            }

            return double.TryParse(str, out value) ? value : (double?)null;
        }

        private static bool TryParseDouble(this string str, ref double value)
        {
            if (str.EndsWith("%"))
            {
                value = double.Parse(str.Substring(0, str.Length - 1)) / 100;
                return true;
            }
            if (str.EndsWith("万"))
            {
                value = double.Parse(str.TrimEnd('万')) * 1_0000;
                return true;
            }
            if (str.EndsWith("亿"))
            {
                value = double.Parse(str.TrimEnd('亿')) * 1_0000_0000;
                return true;
            }

            return false;
        }
    }
}
