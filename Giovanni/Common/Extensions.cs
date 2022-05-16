using System;
using System.Collections.Generic;
using System.Linq;

namespace Giovanni.Common
{
    public static class Extensions
    {
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T[] rest)
        {
            first = array.Length > 0 ? array[0] : default;
            second = array.Length > 0 ? array[1] : default;
            rest = array.Skip(2).ToArray();
        }

        public static void Deconstruct<T>(this IEnumerable<T> array, out T first, out T second, out T third)
        {
            var enumerable = array as T[] ?? array.ToArray();

            first = enumerable.Any() ? enumerable[0] : default;
            second = enumerable.Count() > 1 ? enumerable[1] : default;
            third = enumerable.Count() > 2 ? enumerable[2] : default;
        }

        public static void Deconstruct<T>(this T[] array, out T first, out T second)
        {
            first = array.Length > 0 ? array[0] : default;
            second = array.Length > 1 ? array[1] : default;
        }

        public static bool HasPrefix(this string str, string prefix)
        {
            var slicedStr = str[..prefix.Length];

            return slicedStr == prefix;
        }

        public static bool IsEmpty(this string str) => str == "";

        public static string Capitalize(this string str) => char.ToUpper(str[0]) + str[1..];

        public static Dictionary<string, T> ToStringDictionary<T>(this T[] array, Func<T, string> getName)
        {
            return array.ToDictionary(getName);
        }
    }
}