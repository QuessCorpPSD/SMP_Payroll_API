using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Invoice
{
    public class GstInvoiceGrid
    {
            public int? Invoice_Id { get; set; }
            public string Invoice_Number { get; set; } = "";
            public int? Company_Id { get; set; }
            public string Company_Code { get; set; } = "";
            public int? Cost_Center_Mapping_Id { get; set; }
            public string Map_Name { get; set; } = "";
            public int? City_Id { get; set; }
            public string City_Name { get; set; } = "";
            public int? Financial_Year_Id { get; set; }
            public int? Pay_Period_Id { get; set; }
            public string Pay_Period { get; set; } = "";
            public int? Invoice_Type_Id { get; set; }
            public string InvoiceType { get; set; } = "";
            public DateTime? Invoice_Date { get; set; }
            public string Particulars { get; set; } = "";

            public decimal? Amount { get; set; }
            public decimal? IGST_Percentage { get; set; }
            public decimal? IGST_Amount { get; set; }
            public decimal? Service_Charge { get; set; }
            public decimal? Service_Charge_Amount { get; set; }
            public decimal? Sourcing_Fee { get; set; }
            public decimal? Sourcing_Fee_Amount { get; set; }
            public int? No_Of_Employees { get; set; }
            public decimal? Net_Amount { get; set; }
            public int? Input_No { get; set; }
            public decimal? Employee_PF { get; set; }
            public decimal? Employer_PF { get; set; }

            public string DO_Number { get; set; } = "";
            public string Status { get; set; } = "";
            public bool? IsActive { get; set; }
            public bool? IsLocked { get; set; }
            public int? Group_Detail_Id { get; set; }
            public string Group_Name { get; set; } = "";

            public string? Crn_Number { get; set; }
            public string Crn_IRN_Status { get; set; } = "";
            public string Crn_IRN_Number { get; set; } = "";
            public DateTime? Crn_IRN_Cancel_Date { get; set; }
            public string Sap_Invoice_Number { get; set; } = "";
            public string Sap_Account_Number { get; set; } = "";
            public string Sap_Cancel_Document { get; set; } = "";
            public string Sap_Credit_Note_Document { get; set; } = "";

            public decimal? BGVBL { get; set; }
            public decimal? ASTFEE { get; set; }
            public decimal? DISCT1 { get; set; }
            public decimal? DISCT2 { get; set; }
            public decimal? IDCARD { get; set; }
            public decimal? EMAIL { get; set; }
            public decimal? REGFEE { get; set; }
            public decimal? TRNFEE { get; set; }
            public decimal? GGDBT { get; set; }
            public decimal? PPEKIT { get; set; }
            public decimal? VMSFEE { get; set; }
            public decimal? CALCRG { get; set; }
            public decimal? CALRT { get; set; }
    }
    public class BulkInvoices
    {
        public List<int> invoiceIds { get; set; }
    }

    public class InvoiceTypeUI
    {
        public string Invoice_Type_Id { get; set; } = "";
        public string InvoiceType { get; set; } = "";
    }

    public class BillingTypeUI
    {
        public string Billable_Type_Id { get; set; } = "";
        public string Billable_Type { get; set; } = "";
    }

    public class CtcDeductionUI
    {
        public string Ctc_Deduction_Type_Id { get; set; } = "";
        public string Ctc_Deduction_Type { get; set; } = "";
    }

    public class NewDeductionUI
    {
        public string Net_Deduction_Type_Id { get; set; } = "";
        public string Net_Deduction_Type { get; set; } = "";
    }

    public class GSTPercentageUI
    {
        public string Gst_Percentage { get; set; } = "";
    }

    public class AttributeUI
    {
        public int? id { get; set; }
        public string? AttributeName { get; set; } = string.Empty;
        public string? ActionType { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }

        public string? createdOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }

        public string? Messages { get; set; }

    }

    public class SelectedItems
    {
        public string value { get; set; } = "";
        public string text { get; set; } = "";
    }
}
