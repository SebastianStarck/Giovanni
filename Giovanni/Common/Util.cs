using System;
using System.Collections.Generic;
using System.Linq;

namespace Giovanni.Common
{
    public static class Util
    {
        public static string GetIDFromURL(string url) => new Uri(url).AbsolutePath.Split('/').Last();

        public static string ToString(this IEnumerable<string> items) => items.Aggregate("", ((s, s1) => $"{s},{s1}"));
    }
}