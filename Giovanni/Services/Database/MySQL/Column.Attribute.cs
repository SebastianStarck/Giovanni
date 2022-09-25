using System;

namespace Giovanni.Services.Database.MySQL
{
    public enum ColumnAttributeFields
    {
        Position,
        Type,
        ColumnName,
        IsPivot
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName => _columnName;
        public Type Type => _type;
        public int Position => _position;
        public bool IsPivot => _isPivot;

        private readonly string _columnName;
        private readonly Type _type;
        private readonly bool _isPivot;
        private readonly int _position;

        // TODO: Implement pivot column
        public ColumnAttribute(int position, Type type, string columnName = null, bool isPivot = false)
        {
            _columnName = columnName;
            _position = position;
            _type = type;
            _isPivot = isPivot;
        }
    }
}