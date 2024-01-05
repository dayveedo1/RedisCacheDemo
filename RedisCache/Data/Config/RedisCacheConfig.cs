namespace RedisCache.Data.Config
{
    public static class RedisCacheConfig
    {
        public static IConfiguration AppSetting { get; }

        static RedisCacheConfig()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
