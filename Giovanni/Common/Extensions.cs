using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Discord;
using Discord.Commands;
using Giovanni.Services.Database;

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

        public static T InstantiateCurrent<T>(this DbDataReader dataReader) where T : new()
        {
            var type = typeof(T);
            var entity = new T();

            if (!_typeFields.TryGetValue(type, out var fields))
            {
                fields = type.GetFields()
                    .ToDictionary(entry => entry.Name, entry => entry.CustomAttributes.First(
                        attribute =>
                            attribute.AttributeType == typeof(ColumnAttribute)));

                _typeFields.Add(type, fields);
            }

            foreach (var field in entity.GetType().GetFields())
            {
                fields.TryGetValue(field.Name, out var fieldMeta);
                var (name, index, _) = fieldMeta.ConstructorArguments
                    .Select(attribute => attribute.Value);
                var position = dataReader.GetOrdinal((string) name);


                entity.TrySetField(field.Name, dataReader.GetValue(position));
            }

            return entity;
        }

        private static void TrySetField(this object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName);

            if (field != null) field.SetValue(obj, value);
        }

        public static EmbedBuilder AddEmptyField(this EmbedBuilder builder)
        {
            builder.AddField("\u200B", "\u200B", true);

            return builder;
        }

        public static string GetDescription(this CommandInfo command)
        {
            

            return "Im a description, yey!";
        }
    }
}