using Microsoft.AspNetCore.Mvc;
using RedisCache.Data.Interface;
using RedisCache.Data.Model;

namespace RedisCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisCacheController : Controller
    {
        private readonly RedisCacheDbContext context;
        private readonly IRedisCacheService service;

        private static readonly SemaphoreSlim cacheLock = new SemaphoreSlim(1, 1);


        public RedisCacheController(RedisCacheDbContext context, IRedisCacheService service)
        {
            this.context = context;
            this.service = service;
        }
        

        [HttpGet("products")]
        public IEnumerable <Product> Get()
        {
            var cacheData = service.GetData<IEnumerable<Product>>("product");

            if (cacheData != null)
                return cacheData;

            var expirationTime = DateTimeOffset.Now.AddMinutes(5);

            cacheLock.Wait();

            try
            {
                cacheData = service.GetData<IEnumerable<Product>>("product");

                if (cacheData != null)
                    return cacheData;

                cacheData = context.Products.ToList();

                service.SetData("product", cacheData, expirationTime);
                return cacheData;
            }

            finally
            {
                cacheLock.Release();
            }

            //cacheData = context.Products.ToList();

            //service.SetData("product", cacheData, expirationTime);
            //return cacheData;

        }


        [HttpGet("product")]
        public Product Get(int id)
        {
            Product filteredData;

            var cacheData = service.GetData<IEnumerable<Product>>("product");

            if (cacheData != null)
            {
                filteredData = cacheData.FirstOrDefault(x => x.ProductId == id);
                return filteredData;
            }

            filteredData = context.Products.FirstOrDefault(x => x.ProductId == id);
            return filteredData;
        }


        [HttpPost("addProduct")]
        public async Task<Product> Post(Product product)
        {
            await context.Products.AddAsync(product);
            service.RemoveData("product");

            await context.SaveChangesAsync();

            return product;
        }


        [HttpPut("updateProduct")]
        public void Put(Product product)
        {
            context.Products.Update(product);
            service.RemoveData("product");
            context.SaveChanges();
        }


        [HttpDelete("deleteProduct")]
        public void Delete(int id)
        {
            var filteredData = context.Products.FirstOrDefault(x => x.ProductId == id);
            if (filteredData != null)
            {
                context.Products.Remove(filteredData);
                service.RemoveData("product");
                context.SaveChanges();
            }
        }


    }
}
