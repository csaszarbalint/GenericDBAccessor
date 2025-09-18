using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GenericDBAccessor
{
    internal class DBAccessor<T>
    {
        private struct DbTable
        {
            public string Name;
            public List<string> Fields;
        }

        private MySqlConnection _connection;
        private DbTable _table;
        
        public DBAccessor(MySqlConnection conn)
        { 
            this._connection = conn;

            SetTable(typeof(T));
        } 
    
        //CRUD
        public bool Create(T obj)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<T> Read()
        {
            var result = new List<T>();

            using(var cmd = _connection.CreateCommand())
            {
                cmd.CommandText =
                    $"SELECT * FROM {_table.Name}";
            }

            return result;
        }

        public T Read(int id)
        {
            throw new NotImplementedException();
        }

        public T Update(int id, T obj)
        {
            throw new NotImplementedException();
        }

        public T Delete(int id)
        {
            throw new NotImplementedException();
        }
        private void SetTable(Type t)
        {
            _table = new DbTable();

            //table name
            var tableAttribute = (TableAttribute?)t.GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType() == typeof(TableAttribute));

            _table.Name = tableAttribute != null ? tableAttribute.Name : t.Name.ToLower();

            //fields
            var columnAttributes = t.GetProperties();
            foreach (var e in columnAttributes)
            {
                var columnAttribute = (ColumnAttribute?)e.GetCustomAttributes(false)
                    .FirstOrDefault(a => a.GetType() == typeof(ColumnAttribute));
                
                var columnName = columnAttribute != null ? columnAttribute.Name : e.Name.ToLower();

                if(_table.Fields == null) _table.Fields = new List<string>();
                _table.Fields.Add(columnName);
            }
        }
    }
}
