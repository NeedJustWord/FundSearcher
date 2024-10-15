namespace FundSearcher.Extensions
{
    static class StringExtension
    {
        public static bool IsNotNullAndEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}
