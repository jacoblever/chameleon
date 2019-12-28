using System.Collections.Generic;

namespace GameLogic
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions
    {
        public static string Concat(this IEnumerable<string> items)
        {
            return string.Concat(items);
        }
    }
}
