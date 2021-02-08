using Casino.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.DataAccess
{
    public class RedisDataAccessModel
    {
        private ConnectionMultiplexer connection;
        IDatabase db;

        public bool Connect() 
        {
            if (this.db == null)
            {
                var serverHost = Environment.GetEnvironmentVariable("SERVER_HOST");
                this.connection = ConnectionMultiplexer.Connect(serverHost);
                this.db = this.connection.GetDatabase(); 
            }

            return this.db != null;
        }
        public void Disconnect() 
        {
            this.db = null;
        }

        public bool Save(IDomain obj) 
        {
            string json = JsonConvert.SerializeObject(obj);
            string key = obj.CreateKey();
            bool result = this.db.HashSet(key, obj.Id, json);
            return result;
        }

        public bool Delete(IDomain obj)
        {
            string key = obj.CreateKey();
            bool result = this.db.HashDelete(key, obj.Id);
            return result;
        }

        public IEnumerable<T> FindAll<T>() where T:IDomain
        {
            List<T> list = new List<T>();
            string key = $"{typeof(T)}";
            var values = this.db.HashGetAll(key);
            T obj;
            foreach (var hashEntry in values)
            {
                obj = JsonConvert.DeserializeObject<T>(hashEntry.Value);
                list.Add(obj);
            }
            return list;
        }

        public T Find<T>(int id) 
        {
            string key = $"{typeof(T)}";
            var value = this.db.HashGet(key, id);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }



        
    }
}
