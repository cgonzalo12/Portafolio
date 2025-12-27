using Application.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infraestructure.Data
{
    public class RedisContext : IRedisContext
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisContext(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public IDatabase Database => _database;

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            if (expiry.HasValue)
            {
                return await _database.StringSetAsync(key, json, expiry.Value);
            }
            return await _database.StringSetAsync(key, json);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        // Nuevos métodos para trabajar con listas
        public async Task<List<T>> GetListAsync<T>(string key)
        {
            var values = await _database.ListRangeAsync(key);
            var result = new List<T>();

            foreach (var value in values)
            {
                if (!value.IsNullOrEmpty)
                {
                    result.Add(JsonSerializer.Deserialize<T>(value));
                }
            }

            return result;
        }

        public async Task<bool> AddToListAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.ListRightPushAsync(key, json);
            return true;
        }

        public async Task<bool> RemoveFromListAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.ListRemoveAsync(key, json);
            return true;
        }

        public async Task<long> GetListLengthAsync(string key)
        {
            return await _database.ListLengthAsync(key);
        }

        // Método optimizado para obtener keys (solo usar si es realmente necesario)
        public async Task<List<string>> GetKeysAsync(string pattern)
        {
            try
            {
                var endpoints = _redis.GetEndPoints();
                if (endpoints.Length == 0)
                    return new List<string>();

                var server = _redis.GetServer(endpoints[0]);
                var keys = new List<string>();

                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    keys.Add(key.ToString());
                }

                return keys;
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
