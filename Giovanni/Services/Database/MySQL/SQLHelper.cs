using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Giovanni.Common;

namespace Giovanni.Services.Database.MySQL
{
    internal static class SQLHelper
    {
        public static SQLDecoratedEntity<T> DecorateEntity<T>(T entity)
        {
            var sqlEntity = new SQLDecoratedEntity<T>();
            var columnsMetadata = GetColumnsMetadata(typeof(T));

            foreach (var field in entity.GetType().GetFields())
            {
                var hasMetadata = columnsMetadata.TryGetValue(field.Name, out var fieldMeta);
                if (!hasMetadata) continue;

                var value = field.GetValue(entity);

                var columnName =
                    fieldMeta.ConstructorArguments[(int)ColumnAttributeFields.ColumnName].Value?.ToString()
                    ?? field.Name.ToSnakeCase();

                if (columnName is null || value is null) continue;

                sqlEntity.AddKeyValue(columnName, value.ToString());
            }

            return sqlEntity;
        }

        public static T ClassFromDataReader<T>(IDataRecord dataReader, Type tableType) where T : new()
        {
            var entity = new T();
            var columnsMetadata = GetColumnsMetadata(tableType);

            foreach (var field in entity.GetType().GetFields())
            {
                columnsMetadata.TryGetValue(field.Name, out var fieldMeta);
                var (name, _, _) = fieldMeta
                    .ConstructorArguments
                    .Select(attribute => attribute.Value);

                var position = dataReader.GetOrdinal((string)name);
                TrySetField(entity, field.Name, dataReader.GetValue(position));
            }

            return entity;
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

        internal static T InstantiateEntity<T>(IDataRecord dataReader) where T : new()
        {
            var type = typeof(T);
            var entity = new T();

            Console.WriteLine(dataReader.FieldCount);
            if (!_typeFields.TryGetValue(type, out var fields))
            {
                fields = type.GetFields()
                    .ToDictionary(entry => entry.Name,
                        entry => entry.CustomAttributes.First(
                            attribute => attribute.AttributeType == typeof(ColumnAttribute)));

                _typeFields.Add(type, fields);
            }

            foreach (var field in entity.GetType().GetFields())
            {
                fields.TryGetValue(field.Name, out var fieldMeta);
                var columnPosition = fieldMeta.ConstructorArguments[(int)ColumnAttributeFields.Position].Value;

                TrySetField(entity, field.Name, dataReader.GetValue((int)columnPosition));
            }

            return entity;
        }

        private static void TrySetField(object obj, string property, object value)
        {
            var field = obj.GetType().GetField(property);
            var type = field.FieldType;
            var castedValue = Convert.ChangeType(value, type);
            
            if (field != null) field.SetValue(obj, castedValue);
        }

        internal static string GetTableName(Type type)
        {
            var tableName =
                (type.GetCustomAttributes().FirstOrDefault(attribute => attribute is TableNameAttribute) as
                    TableNameAttribute)
                ?.TableName ?? $"{type.Name.ToSnakeCase()}s";

            return tableName;
        }
    }
}