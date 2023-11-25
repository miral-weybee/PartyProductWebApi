using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class ProductDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
    }
}
