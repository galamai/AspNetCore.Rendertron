
namespace AspNetCore.Rendertron
{
    public static class StringExtensions
    {
        public static string Replace(this string source, string oldValueStart, string oldValueEnd, string newValue)
        {
            var startIndex = source.IndexOf(oldValueStart);
            var endIndex = source.IndexOf(oldValueEnd, startIndex);
            return source
                .Remove(startIndex, endIndex - startIndex)
                .Insert(startIndex, newValue);
        }
    }
}
