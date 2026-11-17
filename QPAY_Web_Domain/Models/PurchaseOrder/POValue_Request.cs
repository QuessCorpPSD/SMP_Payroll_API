namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class POValue_Request
    {
        public string company_Id { get; set; } = "";
        public string site_Id { get; set; } = "";
        public string quantity_type { get; set; } = "";
        public string quantity { get; set; } = "";
        public string po_Rate { get; set; } = "";
    }
}
