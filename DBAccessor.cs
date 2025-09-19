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
using Mysqlx.Crud;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Asn1.Crmf;

namespace GenericDBAccessor
{
    internal class DBAccessor<T>
    {
        internal struct AttributeInfo
        {
            public string Name;
            public PropertyInfo Property;
            public Type AttributeType;
        }
        internal struct DbTable
        {
            public string Name;
            public List<AttributeInfo> AttributeMap;
        }

        private MySqlConnection _connection;
        private DbTable _table;

        private List<T> _cashedData = null;

        public DBAccessor(MySqlConnection conn)
        { 
            this._connection = conn;

            SetTable(typeof(T));
        } 
    
        //CRUD
        public bool Create(T obj)
        { 
            if(obj == null) throw new ArgumentNullException(nameof(obj));

            var keyProperty = _table.AttributeMap
                .FirstOrDefault(a => a.AttributeType == typeof(KeyAttribute))
                .Property;
            var keyValue = keyProperty.GetValue(obj) == null ? throw new ArgumentNullException("Key value cannot be null") : (int)keyProperty.GetValue(obj);
                
            if(keyValue < 0)
                throw new ArgumentException("Key cannot be negative");
            if(KeyExists(keyValue))
                throw new ArgumentException($"An entry with key {keyValue} already exists");

            using (var cmd = _connection.CreateCommand())
            {
                var keyAttribute = _table.AttributeMap
                    .FirstOrDefault(a => a.AttributeType == typeof(KeyAttribute));

                var columnAttributes = _table.AttributeMap
                    .Where(a => a.AttributeType == typeof(ColumnAttribute))
                    .ToList();
                cmd.CommandText =
                    $"INSERT INTO {_table.Name} " +
                    $"({String.Join(", ", columnAttributes.Select(a => a.Name))}) " +
                    $"VALUES ({String.Join(", ", columnAttributes.Select(a => "@" + a.Name))});";
                foreach(var attribute in columnAttributes)
                {
                    var value = attribute.Property.GetValue(obj);
                    cmd.Parameters.AddWithValue("@" + attribute.Name, value);
                }
                var rowsAffected = cmd.ExecuteNonQuery();

                _cashedData = null;
                return rowsAffected > 0;
            }
        }
        public IEnumerable<T> Read()
        {
            if(_cashedData != null)
                return _cashedData;
            var result = new List<T>();

            using(var cmd = _connection.CreateCommand())
            {
                cmd.CommandText =
                    $"SELECT " +
                    $"{String.Join(", ",_table.AttributeMap.Select(a => a.Name))}" +
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
                            attribute.Property.SetValue(obj, reader[offset++]);
                        }

                        result.Add(obj);
                    }
                }
            }
            _cashedData = result;
            return result;
        }

        public T Read(int id)
        {
            using(var cmd = _connection.CreateCommand())
            {
                var keyAttribute = _table.AttributeMap
                    .FirstOrDefault(a => a.AttributeType == typeof(KeyAttribute));

                cmd.CommandText =
                    $"SELECT " +
                    $"{String.Join(", ",_table.AttributeMap.Select(a => a.Name))}" +
                    $" FROM {_table.Name} " +
                    $"WHERE {keyAttribute.Name} = {id};";

                using (var reader = cmd.ExecuteReader()) 
                {
                    if(reader.Read())
                    {
                        T obj = (T)Activator.CreateInstance(typeof(T));
                        if (obj == null) throw new ArgumentNullException();
                        int offset = 0;
                        foreach(var attribute in _table.AttributeMap)
                        {
                            attribute.Property.SetValue(obj, reader[offset++]);
                        }
                        return obj;
                    }
                    else
                    {
                        throw new KeyNotFoundException($"No entry with id {id} found");
                    }
                }
            }
            throw new NotImplementedException();
        }

        public T Update(int id, T obj)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj));
            if(!KeyExists(id))
                throw new KeyNotFoundException($"No entry with id {id} found");

            using(var cmd = _connection.CreateCommand())
            {
                var keyAttribute = _table.AttributeMap
                    .FirstOrDefault(a => a.AttributeType == typeof(KeyAttribute));
                var columnAttributes = _table.AttributeMap
                    .Where(a => a.AttributeType == typeof(ColumnAttribute))
                    .ToList();

                cmd.CommandText =
                    $"UPDATE {_table.Name} SET " +
                    $"{String.Join(", ", columnAttributes.Select(a => a.Name + " = @" + a.Name))} " +
                    $"WHERE {keyAttribute.Name} = {id};";

                foreach(var attribute in columnAttributes)
                {
                    var value = attribute.Property.GetValue(obj);
                    cmd.Parameters.AddWithValue("@" + attribute.Name, value);
                }
                var rowsAffected = cmd.ExecuteNonQuery();

                _cashedData = null;
                return rowsAffected > 0 ? obj : throw new Exception("Update failed");
            }
        }

        public T Delete(int id)
        {
            if (!KeyExists(id))
                throw new KeyNotFoundException($"No entry with id {id} found");
            

        }

        //helpers
        internal bool KeyExists(int id)
        {
            IEnumerable<T> data = _cashedData == null ? Read() : _cashedData;

            var keyAttribute = _table.AttributeMap
                .FirstOrDefault(a => a.AttributeType == typeof(KeyAttribute));
            return data.Select(e => keyAttribute.Property.GetValue(e)).Contains(id);
        }
        internal AttributeInfo TryMatchingProperty(PropertyInfo pinfo)
        {
            var propertyAttributes = pinfo.GetCustomAttributes(false);

            if(propertyAttributes.Length == 0)
            {
                return new AttributeInfo{ Name = pinfo.Name.ToLower(), Property = pinfo, AttributeType = typeof(ColumnAttribute) };
            }

            //has attributes
            if (propertyAttributes.Select(e => e.GetType()).Contains(typeof(KeyAttribute)))
                return new AttributeInfo { Name = pinfo.Name.ToLower(), Property = pinfo, AttributeType = typeof(KeyAttribute) };
                
            if(propertyAttributes.Select(e => e.GetType()).Contains(typeof(ColumnAttribute)))
            {
                var columnAttribute = (ColumnAttribute?)propertyAttributes
                    .FirstOrDefault(a => a.GetType() == typeof(ColumnAttribute));
                var columnName = columnAttribute != null ? columnAttribute.Name : pinfo.Name.ToLower();
                return new AttributeInfo{ Name = columnName, Property = pinfo, AttributeType = typeof(ColumnAttribute)};
            }

            throw new ArgumentException($"No valid attribute found for property {pinfo}");
        }
        internal void SetTable(Type t)
        {
            _table = new DbTable();
            _table.AttributeMap = new List<AttributeInfo>();

            //table name
            var tableAttribute = (TableAttribute?)t.GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType() == typeof(TableAttribute));

            _table.Name = tableAttribute != null ? tableAttribute.Name : t.Name.ToLower();

            //fields
            var columnAttributes = t.GetProperties();
            foreach (var e in columnAttributes)
            {
                _table.AttributeMap.Add(TryMatchingProperty(e));
            }

            if (!_table.AttributeMap.Select(a => a.AttributeType).Contains(typeof(KeyAttribute)))
            {
                throw new Exception($"Key attribute not found!");
            }
        }

    }
}
