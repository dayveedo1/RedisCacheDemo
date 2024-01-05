using System.ComponentModel.DataAnnotations;

namespace RedisCache.Data.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int Stock { get; set; }
    }
}
