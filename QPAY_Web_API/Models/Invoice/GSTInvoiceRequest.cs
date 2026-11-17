namespace QPay.API.Models.Invoice
{
    public class GSTInvoiceRequest
    {
        public int Invoice_Id { get; set; }
        public int Invoice_Number { get; set; }
        public Int32 Company_Id { get; set; }
        public Int32 Cost_Center_Mapping_Id { get; set; }
        public Int32? City_Id { get; set; }
        public string City_Name { get; set; } = "";
        public Int32 Financial_Year_Id { get; set; }
        public Int32 Pay_Period_Id { get; set; }
        public Int32 Invoice_Type_Id { get; set; }
        public DateTime Invoice_Date { get; set; }
        public DateTime Invoice_Due_Date { get; set; }
        public String Particulars { get; set; } = "";
        public Decimal Amount { get; set; }
        public Int32 StateId { get; set; }
        public Decimal CGST_Percentage { get; set; }
        public Decimal SGST_Percentage { get; set; }
        public Decimal UTGST_Percentage { get; set; }
        public Decimal IGST_Percentage { get; set; }
        public Boolean Client_PO { get; set; }
        public Int32? Purchase_Order_Id { get; set; }
        public DateTime Input_Date { get; set; }
        public DateTime Output_Date { get; set; }
        public Decimal Service_Charge { get; set; }
        public Decimal Service_Charge_Amount { get; set; }
        public Decimal Sourcing_Fee { get; set; }
        public Decimal Sourcing_Fee_Amount { get; set; }
        public Int32 No_Of_Employees { get; set; }
        public Decimal Absorption_Fee { get; set; }
        public Decimal Absorption_Amt { get; set; }
        public Decimal CTC_Amt_Adjusted { get; set; }
        public Boolean CTC_Amt_NorP { get; set; }
        public String CTC_Adj_Note { get; set; } = "";
        public Decimal Net_Amt_Adjusted { get; set; }
        public Boolean Net_Amt_NorP { get; set; }
        public String Net_Adj_Note { get; set; } = "";
        public Int32 Invoice_Culture_Id { get; set; }
        public String Invoice_Culture_RefNo { get; set; } = "";
        public Int32? Input_No { get; set; }
        public Decimal Employee_ESI { get; set; }
        public Decimal Employer_ESI { get; set; }
        public Decimal Employee_PF { get; set; }
        public Decimal Employer_PF { get; set; }
        public Decimal Mobile_Recovery_Amount { get; set; }
        public Decimal Personal_Loan_Amount { get; set; }
        public Decimal Other_Deduction_Amount { get; set; }
        public String WO_Number { get; set; } = "";
        public String Pl_Id_No { get; set; } = "";
        public String Employee_Name { get; set; } = "";
        public Decimal Markup { get; set; }
        public Decimal Gri_Msp { get; set; }
        public String DO_Number { get; set; } = "";
        public String Created_Mode { get; set; } = "";
        public String Remarks { get; set; } = "";
        public String Status { get; set; } = "";
        public Boolean IsActive { get; set; }
        public DateTime? WO_Date { get; set; }
        public String InvoiceNotes { get; set; } = "";
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Boolean? IsLocked { get; set; }
        public String Discrepancy_By { get; set; } = "";
        public Decimal Onboarding_Charge { get; set; }
        public Int32? Group_Detail_Id { get; set; }
        public Decimal TaxableAmount1 { get; set; }
        public String TaxableAmount1_Note { get; set; } = "";
        public Decimal NonTaxableAmount1 { get; set; }
        public Decimal NonTaxableAmount2 { get; set; }
        public String NonTaxableAmount2_Note { get; set; } = "";
        public Decimal NonTaxableAmount3 { get; set; }
        public String NonTaxableAmount3_Note { get; set; } = "";
        public Decimal CGST_Amount { get; set; }
        public Decimal SGST_Amount { get; set; }
        public Decimal UTGST_Amount { get; set; }
        public Decimal IGST_Amount { get; set; }
        public Decimal Net_Amount { get; set; }
        public Decimal BGVBL { get; set; }
        public Decimal ASTFEE { get; set; }
        public Decimal DISCT1 { get; set; }
        public Decimal DISCT2 { get; set; }
        public Decimal IDCARD { get; set; }
        public Decimal EMAIL { get; set; }
        public Decimal REGFEE { get; set; }
        public Decimal TRNFEE { get; set; }
        public Decimal GGDBT { get; set; }
        public Decimal PPEKIT { get; set; }
        public Decimal VMSFEE { get; set; }
        public Decimal EDUFEE { get; set; }
        public Decimal NTPRY { get; set; }
        public Decimal DRADED { get; set; }
        public Decimal RENMAC { get; set; }
        public Decimal OTHDD { get; set; }
        public Decimal CALCRG { get; set; }
        public Decimal CALRT { get; set; }
    }
   
}
