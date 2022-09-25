using System.Collections.Generic;
using Giovanni.Common;

namespace Giovanni.Services.Database.MySQL
{
    public class SQLDecoratedEntity<T>
    {
        private Dictionary<string, string> entries = new();
        private readonly string _tableName;

        public SQLDecoratedEntity() => _tableName = SQLHelper.GetTableName(typeof(T));

        public SQLDecoratedEntity<T> AddKeyValue(string key, string value)
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