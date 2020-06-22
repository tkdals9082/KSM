namespace KSM.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtension
    {
        public static IEnumerable<TResult> ConvertAll<TSource, TResult>(this IEnumerable<TSource> list, Func<TSource, TResult> func) => list.Select(func);

        public static void ForEach_Terminate<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach(T t in list)
            {
                action(t);
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach(T t in list)
            {
                action(t);
                yield return t;
            }
        }
    }
}