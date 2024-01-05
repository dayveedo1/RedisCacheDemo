using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RedisCache.Data.Interface;
using StackExchange.Redis;

namespace RedisCache.Data.Service
{
    public class RedisCacheService : IRedisCacheService
    {
        private IDatabase database;

        public RedisCacheService()
        {
            ConfigureRedis();
        }

        private void ConfigureRedis()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");

            database = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = database.StringGet(key);
            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);

            return default;
        }

        public object RemoveData(string key)
        {
            bool isKeyExists = database.KeyExists(key);
            if (isKeyExists)
                return database.KeyDelete(key);

            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

            var isSet = database.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
    }
}
