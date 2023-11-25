using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class InvoiceAddDTO
    {
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public int CurrentRate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int PartyPartyId { get; set; }
        [Required]
        public int ProductProductId { get; set; }
        
        public DateOnly? Date { get; set; }
    }
}
