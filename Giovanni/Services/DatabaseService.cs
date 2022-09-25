using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Giovanni.Common;
using Giovanni.Services.Database.MySQL;
using MySql.Data.MySqlClient;

namespace Giovanni.Services
{
    public class DatabaseService
    {
        public async Task<Tuple<DbDataReader, MySqlCommand>> RunQuery(string query, bool isInsert = false)
        {
            var connection = DBConnection.Instance().Open();
            var command = new MySqlCommand(query, connection);
            var result = await command.ExecuteReaderAsync();

            return new Tuple<DbDataReader, MySqlCommand>(result, command);
        }

        // public async Task<T> InsertOne<T>(T entity) where T : new()
        public async Task<T> InsertOne<T>(T entity) where T : new()
        {
            var decoratedEntity = SQLHelper.DecorateEntity(entity);

            try
            {
                var (dataReader, command) = await RunQuery(decoratedEntity.ToInsertQuery());
                var entityID = command.LastInsertedId;

                command.Connection.Close();
                return await GetOne<T>((int)entityID);
            }
            catch (Exception e)
            {
                // TODO: Log into LogService
                Console.WriteLine(e);
            }

            return default;
        }

        public async Task<T> GetOne<T>(int id) where T : new()
        {
            var tableName = SQLHelper.GetTableName(typeof(T));
            var (dataReader, _) = await RunQuery($"SELECT * FROM {tableName} WHERE id = {id}");
            dataReader.Read();

            return SQLHelper.InstantiateEntity<T>(dataReader);
        }

        public async Task<List<T>> GetMany<T>() where T : new()
        {
            var tableName = SQLHelper.GetTableName(typeof(T));
            var (dataReader, _) = await RunQuery($"SELECT * FROM {tableName}");
            var entities = new List<T>();

            while (dataReader.Read()) entities.Add(SQLHelper.InstantiateEntity<T>(dataReader));

            return entities;
        }
    }
}