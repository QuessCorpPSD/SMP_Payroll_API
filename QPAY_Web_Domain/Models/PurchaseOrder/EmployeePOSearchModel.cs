namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class EmployeePOSearchModel
    {
        public int companyId { get; set; }
        public string poNumber { get; set; } = "";
        public string employeeId { get; set; } = "";
        public string status { get; set; } = "";
        public string pricingType { get; set; } = "";

    }

    public class EmployeePoViewApprove
    {
        public int companyId { get; set; }
        public string poNumber { get; set; } = "";
        public string pricingType { get; set; } = "";
        public string employeeId { get; set; } = "";
        public string status { get; set; } = "";
        public string employeeListId { get; set; } = "";

    }

    public class EmployeePoView
    {
        public int companyId { get; set; }
        public string poNumber { get; set; } = "";
        public string clientEmployeeId { get; set; } = "";
        public string status { get; set; } = "";
        public string pricingType { get; set; } = "";

    }

    public class EmployeePoViewEmpDtls
    {
        public int companyId { get; set; }
        public string poNumber { get; set; } = "";
        public string pricingType { get; set; } = "";
        public string employeeId { get; set; } = "";

    }

    public class EmployeePoViewApproveNew
    {
        public int companyId { get; set; }
        public string poNumber { get; set; } = "";
        public string pricingType { get; set; } = "";
        public string employeeId { get; set; } = "";
        public string status { get; set; } = "";

    }
    
}
