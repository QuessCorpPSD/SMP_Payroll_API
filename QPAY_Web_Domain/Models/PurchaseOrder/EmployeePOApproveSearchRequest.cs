namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class EmployeePOApproveSearchRequest
    {
        public string companyId { get; set; } = "";
        public string siteId { get; set; } = "";
        public string poNumber { get; set; } = "";
        public string pricingType { get; set; } = "";
        public string employeeId { get; set; } = "";
        public string status { get; set; } = "";
    }
}
