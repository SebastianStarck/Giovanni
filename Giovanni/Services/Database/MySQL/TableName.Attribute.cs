using System;

namespace Giovanni.Services.Database.MySQL
{
    [AttributeUsage(AttributeTargets.All)]
    public class TableNameAttribute : Attribute
    {
        private readonly string _tableTableName;

        public TableNameAttribute(string tableName) => _tableTableName = tableName;

        public string TableName => _tableTableName;
    }
}