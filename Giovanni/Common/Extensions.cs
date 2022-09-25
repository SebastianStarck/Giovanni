using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Discord;
using Discord.Commands;
using Giovanni.Services.Database;
using Giovanni.Services.Database.MySQL;

namespace Giovanni.Common
{
    public static class Extensions
    {
        private static Dictionary<Type, Dictionary<string, CustomAttributeData>> _typeFields = new();

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

        public static EmbedBuilder AddEmptyField(this EmbedBuilder builder)
        {
            builder.AddField("\u200B", "\u200B", true);

            return builder;
        }

        public static string GetDescription(this CommandInfo command)
        {
            var attribute = command.Attributes.FirstOrDefault(attribute => attribute is DescriptionAttribute) as
                DescriptionAttribute;

            return attribute?.Description ?? "";
        }

        public static string JoinToString<T>(
            this IEnumerable<T> enumerable, string glue = ",", string wrapper = ""
        )
        {
            var decoratedValues = enumerable.Select(value => $"{wrapper}{value}{wrapper}");
            return string.Join(glue, decoratedValues);
        }

        // TODO: Implement own wrote code instead of this
        public static string ToSnakeCase(this string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (text.Length < 2) return text;

            var builder = new StringBuilder();
            builder.Append(char.ToLowerInvariant(text[0]));

            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    builder.Append('_');
                    builder.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}