using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class InvoiceAddDTO
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int CurrentRate { get; set; }
        [Required]
        public int Quantity { get; set; }

        public int PartyId { get; set; }

        public int ProductId { get; set; }

        public DateOnly? Date { get; set; }
    }
}
