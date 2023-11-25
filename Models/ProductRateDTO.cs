using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PartyProductWebApi.Models
{
    public class ProductRateDTO
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public DateTime DateOfRate { get; set; }
        [Required]
        public int ProductRateId { get; set; }

    }
}
