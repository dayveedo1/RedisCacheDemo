using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisCache.Data.Config
{
    public class RedisCacheConHelper
    {
        //private readonly IConfiguration configuration;

        //public RedisCacheConHelper(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}

        static RedisCacheConHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("127.0.0.1:6379");

            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
