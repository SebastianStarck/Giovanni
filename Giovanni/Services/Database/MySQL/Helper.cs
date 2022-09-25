using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Giovanni.Common;

namespace Giovanni.Services.Database.MySQL
{
    internal static class Helper
    {
        public static SQLEntity<T> EntityToSQL<T>(T entity)
        {
            var sqlEntity = new SQLEntity<T>();
            var columnsMetadata = GetColumnsMetadata(typeof(T));

            foreach (var field in entity.GetType().GetFields())
            {
                var hasMetadata = columnsMetadata.TryGetValue(field.Name, out var fieldMeta);
                if (!hasMetadata) continue;

                var value = field.GetValue(entity);
                var (columnName, _, _) = fieldMeta
                    .ConstructorArguments
                    .Select(attribute => attribute.Value);

                sqlEntity.AddKeyValue(columnName.ToString(), value.ToString());
            }

            return sqlEntity;
        }

        public static T ClassFromDataReader<T>(IDataRecord dataReader, Type tableType) where T : new()
        {
            var entity = new T();
            var columnsMetadata = Helper.GetColumnsMetadata(tableType);

            foreach (var field in entity.GetType().GetFields())
            {
                columnsMetadata.TryGetValue(field.Name, out var fieldMeta);
                var (name, _, _) = fieldMeta
                    .ConstructorArguments
                    .Select(attribute => attribute.Value);

                var position = dataReader.GetOrdinal((string)name);
                Helper.TrySetProperty(entity, field.Name, dataReader.GetValue(position));
            }

            return entity;
        }

        internal static void TrySetProperty(object obj, string property, object value)
        {
            var field = obj.GetType().GetField(property);

            if (field != null) field.SetValue(obj, value);
        }

        internal static string GetTableName(Type type)
        {
            var tableName =
                (type.GetCustomAttributes().FirstOrDefault(attribute => attribute is TableNameAttribute) as
                    TableNameAttribute)
                ?.TableName ?? $"{type.Name.ToSnakeCase()}s";

            return tableName;
        }

        /// <summary>
        /// Cache of types
        /// </summary>
        private static Dictionary<Type, Dictionary<string, CustomAttributeData>> _typeFields = new();

        internal static Dictionary<string, CustomAttributeData> GetColumnsMetadata(Type tableType)
        {
            if (!_typeFields.TryGetValue(tableType, out var fields))
            {
                fields = tableType.GetFields()
                    .ToDictionary(entry => entry.Name, entry => entry.CustomAttributes.First(
                        attribute =>
                            attribute.AttributeType == typeof(ColumnAttribute)));

                _typeFields.Add(tableType, fields);
            }

            return fields;
        }
    }

    public class SQLEntity<T>
    {
        private Dictionary<string, string> entries = new Dictionary<string, string>();
        private string _tableName;
        private Type _type;

        public SQLEntity()
        {
            _type = typeof(T);
            _tableName = Helper.GetTableName(_type);
        }

        public SQLEntity<T> AddKeyValue(string key, string value)
        {
            entries.Add(key, value);

            return this;
        }

        public string ToInsertQuery()
        {
            var columns = entries.Keys.JoinToString(",", "`");
            var values = entries.Values.JoinToString(",", "'");

            return $"INSERT INTO {_tableName}\n" +
                   $"({columns})\n" +
                   $"VALUES ({values})";
        }
    }
}