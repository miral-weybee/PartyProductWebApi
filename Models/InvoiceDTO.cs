using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class InvoiceDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CurrentRate { get; set; }
        [Required]
        public int Quantity { get; set; }
        
        public string? PartyName { get; set; }
        
        public string? ProductName { get; set; }
        
        public DateOnly? Date { get; set; }

        public int Total { get; set; }

    }
}
