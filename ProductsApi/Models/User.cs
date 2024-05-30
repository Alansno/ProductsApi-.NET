using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public byte[] Salt { get; set; }
        public ICollection<Product> Products { get; } = new List<Product>();
    }
}
