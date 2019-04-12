using Microsoft.Extensions.Caching.Memory;
using System;

namespace qr_backend.Services.Cache
{
    public class Cache : ICache
    {
        private IMemoryCache _memoryCache;

        public Cache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add(string key, object value, DateTimeOffset absExpiration)
        {
            _memoryCache.Set(key, value, absExpiration);
        }

        public void Delete(string key)
        {
            _memoryCache.Remove(key);
        }

        public object Get(string key)
        {
           return _memoryCache.Get<object>(key);
        }
    }
}
