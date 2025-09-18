using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;

namespace GenericDBAccessor
{
    internal class DBAccessor<T>
    {
        private struct DbTable
        {
            public string Name;
            public Dictionary<string, PropertyInfo> AttributeMap;
            public (string, PropertyInfo) Key;
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
                    $"SELECT " +
                    $"{String.Join(", ",_table.AttributeMap.Keys)}" +
                    $" FROM {_table.Name};";

                using (var reader = cmd.ExecuteReader()) 
                {
                    while(reader.Read())
                    {
                        T obj = (T)Activator.CreateInstance(typeof(T));
                        if (obj == null) throw new ArgumentNullException();

                        int offset = 0;
                        foreach(var attribute in _table.AttributeMap)
                        {
                            attribute.Value.SetValue(obj, reader[offset++]);
                        }

                        result.Add(obj);
                    }
                }
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
        internal void SetTable(Type t)
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
                var propertyAttributes = e.GetCustomAttributes(false);
                
                if(propertyAttributes.Select(e => e.GetType()).Contains(typeof(KeyAttribute))) _table.Key = ()

                var columnAttribute = (ColumnAttribute?)propertyAttributes
                    .FirstOrDefault(a => a.GetType() == typeof(ColumnAttribute));
                
                var columnName = columnAttribute != null ? columnAttribute.Name : e.Name.ToLower();

                if(_table.AttributeMap == null) _table.AttributeMap = new Dictionary<string, PropertyInfo>();
                _table.AttributeMap.Add(columnName, e);
            }
        }
    }
}
