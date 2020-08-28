namespace System.Collections.Generic
{
    using System.Linq;
    static class EnumerrableExtesinon
    {
        public static string Join(this IEnumerable<string> source, string separator)
        {
#if NET35
            return string.Join(separator, source.ToArray());
#else
            return string.Join(separator, source);
#endif
        }
    }
}
