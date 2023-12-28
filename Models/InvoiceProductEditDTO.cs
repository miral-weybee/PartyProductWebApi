namespace PartyProductWebApi.Models
{
    public class InvoiceProductEditDTO
    {
        public int InvoiceId { get; set; }
        public List<EditInvoiceProducts> Products { get; set; }
    }
    public class EditInvoiceProducts
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        
    }
}
