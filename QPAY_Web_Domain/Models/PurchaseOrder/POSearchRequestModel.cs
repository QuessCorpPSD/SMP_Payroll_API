namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class POSearchRequestModel
    {
        public int companyID { get; set; }
        public string pricingType { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public string flag { get; set; } = string.Empty;
    }
}
