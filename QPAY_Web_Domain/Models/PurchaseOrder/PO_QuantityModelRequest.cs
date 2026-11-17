namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class PO_QuantityModelRequest
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? QUANTITYTYPE {  get; set; }

        public int CompanyId { get; set; }

    }
}
