namespace Isitar.DependencyUpdater.Application.Common
{
    public static class StringExtensions
    {
        public static string TrimToNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }
        
        public static string FirstCharacterToLower(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
            {
                return str;
            }

            return char.ToLowerInvariant(str[0]) + str[1..];
        }
    }
}