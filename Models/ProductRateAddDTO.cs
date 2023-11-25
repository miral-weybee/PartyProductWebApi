using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class ProductRateAddDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public DateTime DateOfRate { get; set; }
        [Required]
        public int ProductRateId { get; set; }
    }
}
