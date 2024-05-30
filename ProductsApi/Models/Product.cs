using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public required decimal Price { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null;
    }
}
