using System.Collections.Generic;

namespace Ryocatusn.Util
{
    public static class MyList
    {
        public static bool ContainsArray<T>(this List<T> list, T[] items)
        {
            foreach (T item in items)
            {
                if (!list.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
