namespace PartyProductWebApi.Models
{
    public class InvoiceAddDTO
    {
        public int PartyId { get; set; }

        public DateOnly Date { get; set; } = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        public List<InvoiceItemDTO> Products { get; set; } = new List<InvoiceItemDTO>();
    }

    public class InvoiceItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
    }
}
