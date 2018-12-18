using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BridgeportClaims.Common.Extensions
{
    public static class FunctionalExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> array, Action<T> act)
        {
            if (null == array)
            {
                return null;
            }
            var forEach = array as T[] ?? array.ToArray();
            foreach (var item in forEach)
            {
                act(item);
            }
            return forEach;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable arr, Action<T> act)
        {
            return arr?.Cast<T>().ForEach<T>(act);
        }

        public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> array, Func<T, TResult> map)
        {
            return array?.Select(map).Where(obj => null != obj).ToList();
        }
    }
}