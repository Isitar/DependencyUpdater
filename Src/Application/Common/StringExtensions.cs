namespace Isitar.DependencyUpdater.Application.Common
{
    public static class StringExtensions
    {
        public static string TrimToNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }
    }
}