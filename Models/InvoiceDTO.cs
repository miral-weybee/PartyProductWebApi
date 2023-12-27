namespace PartyProductWebApi.Models
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }

        public int PartyId { get; set; }

        public string PartyName { get; set; }

        public DateOnly Date { get; set; }
    }
}
