using System;

namespace Giovanni.Services.Database.MySQL
{
    [AttributeUsage(AttributeTargets.All)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName => _columnName;
        public Type Type => _type;
        public int Position => _position;

        private readonly string _columnName;
        private readonly Type _type;
        private readonly int _position;

        public ColumnAttribute(string columnNameName, int position, Type type)
        {
            _columnName = columnNameName;
            _position = position;
            _type = type;
        }
    }
}