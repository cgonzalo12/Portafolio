using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRedisContext
    {
        IDatabase Database { get; }
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
        Task<List<T>> GetListAsync<T>(string key);
        Task<bool> AddToListAsync<T>(string key, T value);
        Task<bool> RemoveFromListAsync<T>(string key, T value);
        Task<long> GetListLengthAsync(string key);
        Task<List<string>> GetKeysAsync(string pattern);
    }
}
