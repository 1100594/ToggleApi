using System.Collections.Generic;

namespace ToggleApi.Utilities
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object originalObject)
        {
            return originalObject == null;
        }
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
