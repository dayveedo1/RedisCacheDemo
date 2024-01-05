using Microsoft.EntityFrameworkCore;
using RedisCache.Data.Model;

namespace RedisCache
{
    public class RedisCacheDbContext :  DbContext
    {
        public RedisCacheDbContext(DbContextOptions<RedisCacheDbContext> options) : base(options)
        {
        }


        #region EF Entities
        public DbSet<Product> Products { get; set; }

        #endregion

    }
}
