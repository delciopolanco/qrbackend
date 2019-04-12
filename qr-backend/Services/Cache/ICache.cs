
using System;

namespace qr_backend.Services.Cache
{
    public  interface ICache
    {
        void Add(string key, object value, DateTimeOffset absExpiration);

        void Delete(string key);

        object Get(string key);
    }
}
