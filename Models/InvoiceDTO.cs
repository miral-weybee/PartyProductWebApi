namespace PartyProductWebApi.Models
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }

        public string PartyName { get; set; }

        public DateOnly Date { get; set; }
    }
}
