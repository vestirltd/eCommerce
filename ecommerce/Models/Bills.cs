namespace ecommerce.Models
{
    public class Bills
    {
        public string InvoiceNumber { get; set; }
        public string? InvoiceDate { get; set; }
        public string? CustomerName { get; set; }
        public string? product { get; set; }
        public string? price { get; set; }
        public string? quantity { get; set; }
        public int NetTotal { get; set; }
        public double VAT { get; set; }
        public double GrossTotal { get; set; }
    }
}