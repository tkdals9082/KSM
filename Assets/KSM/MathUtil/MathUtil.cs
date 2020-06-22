namespace KSM.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MathUtil
    {
        public static (T min, T max) MinMax<T>(IEnumerable<T> enumerable) where T : IComparable<T>
        {
            if (enumerable.Count() == 0) return (default(T), default(T));

            T min = default(T);
            T max = default(T);

            foreach(var value in enumerable)
            {
                if (min.CompareTo(value) > 0) min = value;
                if (max.CompareTo(value) < 0) max = value;
            }

            return (min, max);
        }
    }
}