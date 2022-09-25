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
using Giovanni.Services.Database.MySQL;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using static System.Int32;
using Type = System.Type;

namespace Giovanni.Services
{
    public class DatabaseService
    {
        public Task<DbDataReader> RunQuery(string query)
        {
            var command = new MySqlCommand(query, DBConnection.Instance().Open());

            return Task.FromResult<DbDataReader>(command.ExecuteReader());
        }

        // public async Task<T> InsertOne<T>(T entity) where T : new()
        public void InsertOne<T>(T entity)
        {
            var parsedEntity = Helper.EntityToSQL(entity);
            Console.WriteLine(parsedEntity.ToInsertQuery());
        }

        public async Task<List<T>> GetMany<T>() where T : new()
        {
            var tableName = Helper.GetTableName(typeof(T));
            var dataReader = await RunQuery($"SELECT * FROM {tableName}");
            var entities = new List<T>();

            while (dataReader.Read()) entities.Add(dataReader.InstantiateCurrent<T>());

            return entities;
        }
    }
}