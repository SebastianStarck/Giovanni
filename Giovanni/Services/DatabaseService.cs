using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Giovanni.Common;
using Giovanni.Services.Database;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using static System.Int32;

namespace Giovanni.Services
{
    public class DatabaseService
    {
        public async Task<DbDataReader> RunQuery(string query)
        {
            var command = new MySqlCommand(query, DBConnection.Instance().Open());


            return command.ExecuteReader();
        }

        public async Task GetMany<T>() where T : new()
        {
            var tableType = typeof(T);
            var tableName =
                (tableType.GetCustomAttributes().First(attribute => attribute is TableNameAttribute) as
                    TableNameAttribute)
                ?.TableName ?? $"{typeof(T).Name.ToLower()}s";

            var dataReader = await RunQuery($"SELECT * FROM {tableName}");
            var entities = new List<T>();

            while (dataReader.Read())
            {
                var entity = new T();
                var columnsMetadata = tableType.GetFields()
                    .ToDictionary(entry => entry.Name, entry => entry.CustomAttributes.First(
                        attribute =>
                            attribute.AttributeType == typeof(ColumnAttribute)));

                foreach (var field in entity.GetType().GetFields())
                {
                    columnsMetadata.TryGetValue(field.Name, out var fieldMeta);
                    var (name, index, type) = fieldMeta.ConstructorArguments
                        .Select(attribute => attribute.Value);
                    var position = dataReader.GetOrdinal((string) name);

                    TrySetProperty(entity, field.Name, dataReader.GetValue(position));
                }

                entities.Add(entity);
            }

            foreach (var entity in entities)
            {
                Console.WriteLine(entity);
            }
        }

        private void TrySetProperty(object obj, string property, object value)
        {
            var field = obj.GetType().GetField(property);

            if (field != null) field.SetValue(obj, value);
        }
    }
}