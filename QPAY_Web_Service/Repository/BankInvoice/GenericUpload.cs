using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using QPay.BAL.IRepository.BankInvoice;
using QPay.DAL.Repository;
using System.Data;

namespace QPay.BAL.Repository.BankInvoice
{
    public class GenericUpload : IGenericUpload
    {
        private readonly DbRepository _dbRepository;

        public GenericUpload(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<DataSet> GetUploadTypes(int createdBy)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Description"] = null,
                ["@Action"] = "BankInvoiceGenericUploadTypes",
                ["@CreatedBy"] = createdBy
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_CommonDropDowns",
                parameters,
                1500
            );
        }

        public async Task<DataSet> UploadFile(IFormFile? file, int createdBy, string? uploadType)
        {
            var validationDs = ValidateFile(file, createdBy, uploadType);

            if (validationDs.Tables[0].Rows.Count > 0)
                return validationDs;

            DataTable expectedColumns = GetExcelColumnNames(uploadType!);

            if (expectedColumns.Columns.Count == 0)
                return ErrorDataSet("Invalid UploadType.");

            DataTable excelData = ReadExcelToDataTable(file!);

            var columnValidation = ValidateExcelColumns(excelData, expectedColumns);

            if (columnValidation.Tables[0].Rows.Count > 0)
                return columnValidation;

            string xmlData = ConvertDataTableToXml(excelData);

            var parameters = new Dictionary<string, object?>
            {
                ["@XML_File"] = xmlData,
                ["@CreatedBy"] = createdBy,
                ["@UploadType"] = uploadType
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_Upload_PartialHoldEmployeeSalary",
                parameters,
                1500
            );
        }

        private DataSet ValidateFile(IFormFile? file, int createdBy, string? uploadType)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Error_Message");

            if (file == null || file.Length == 0)
                dt.Rows.Add("Please select file.");

            if (createdBy <= 0)
                dt.Rows.Add("CreatedBy is required.");

            if (string.IsNullOrWhiteSpace(uploadType))
                dt.Rows.Add("UploadType is required.");

            if (file != null)
            {
                string extension = Path.GetExtension(file.FileName).ToLower();

                if (extension != ".xls" && extension != ".xlsx")
                    dt.Rows.Add("Only .xls and .xlsx files are allowed.");
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        private DataTable ReadExcelToDataTable(IFormFile file)
        {
            DataTable dt = new DataTable();

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            bool firstRow = true;

            foreach (var row in worksheet.RowsUsed())
            {
                if (firstRow)
                {
                    foreach (var cell in row.CellsUsed())
                    {
                        dt.Columns.Add(cell.GetString().Trim());
                    }

                    firstRow = false;
                }
                else
                {
                    DataRow dataRow = dt.NewRow();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dataRow[i] = row.Cell(i + 1).GetString().Trim();
                    }

                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }

        private DataSet ValidateExcelColumns(DataTable excelData, DataTable expectedColumns)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Error_Message");

            foreach (DataColumn expectedColumn in expectedColumns.Columns)
            {
                if (!excelData.Columns.Contains(expectedColumn.ColumnName))
                {
                    dt.Rows.Add("Columns Name are not matching. Missing column: " + expectedColumn.ColumnName);
                }
            }

            foreach (DataColumn uploadedColumn in excelData.Columns)
            {
                if (!expectedColumns.Columns.Contains(uploadedColumn.ColumnName))
                {
                    dt.Rows.Add("Columns Name are not matching. Extra column: " + uploadedColumn.ColumnName);
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        private string ConvertDataTableToXml(DataTable dt)
        {
            dt.TableName = "Table";

            DataSet ds = new DataSet("NewDataSet");
            ds.Tables.Add(dt.Copy());

            using StringWriter sw = new StringWriter();
            ds.WriteXml(sw);

            return sw.ToString();
        }

        private DataSet ErrorDataSet(string message)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Error_Message");
            dt.Rows.Add(message);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        public DataTable GetExcelColumnNames(string uploadType)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrWhiteSpace(uploadType))
                return dt;

            string key = uploadType
    .Trim()
    .Replace(" ", "")
    .Replace("_", "")
    .Replace("-", "")
    .ToLower();

            if (key == "partialholdsalary")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("HoldAmount");
                dt.Columns.Add("SalaryType");
                dt.Columns.Add("HoldReason");
            }
            else if (key == "accountnumberrequest")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
            }
            else if (key == "cancelinvoicemapping")
            {
                dt.Columns.Add("Invoice_No");
                dt.Columns.Add("Employee_code");
                dt.Columns.Add("New_Invoice_No");
            }
            else if (key == "partialholdsalaryrelease")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("PartialReleaseAmount");
                dt.Columns.Add("SalaryType");
            }
            else if (key == "bonusrelease")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
            }
            else if (key == "bankinvoicemanualupload")
            {
                dt.Columns.Add("Company_Code");
                dt.Columns.Add("Invoice_No");
                dt.Columns.Add("Employee_Code");
                dt.Columns.Add("Pay_Period");
                dt.Columns.Add("Bank_Name");
                dt.Columns.Add("Account_No");
                dt.Columns.Add("IFSC_Code");
                dt.Columns.Add("Net_Pay");
                dt.Columns.Add("CTC");
                dt.Columns.Add("Data_From");
                dt.Columns.Add("Input_no");
            }
            else if (key == "manualbatchcreation")
            {
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("SalaryReleaseDate");
            }
            else if (key == "baddebtupdate")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("BadDebtAmount");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("ApprovedBy");
            }
            else if (key == "beneficiaryupdate")
            {
                dt.Columns.Add("EmployeeCode");
            }
            else if (key == "multipleclientinvoicemapping")
            {
                dt.Columns.Add("Invoice_No");
                dt.Columns.Add("Employee_code");
                dt.Columns.Add("New_Invoice_No");
            }
            else if (key == "invoiceadhoc")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("Particulars");
                dt.Columns.Add("NoofBeneficiaries");
                dt.Columns.Add("Amount_Beneficiary");
            }
            else if (key == "creditnoteadjustment")
            {
                dt.Columns.Add("CreditNoteNumber");
                dt.Columns.Add("AdjustedAmount");
            }
            else if (key == "mapnamechanges")
            {
                dt.Columns.Add("COMPANY_CODE");
                dt.Columns.Add("EMPLOYEE_CODE");
                dt.Columns.Add("PAY_PERIOD");
                dt.Columns.Add("ACTION");
            }
            else if (key == "dbtholdsalary")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("HoldAmount");
                dt.Columns.Add("SalaryType");
            }
            else if (key == "dbtholdsalaryrelease")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("DBTReleaseAmount");
                dt.Columns.Add("SalaryType");
            }
            else if (key == "blockcnamounthold")
            {
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("CreditNoteNo");
                dt.Columns.Add("Amount");
                dt.Columns.Add("HoldStatus");
            }
            else if (key == "partialholdreversal")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("ReversalAmount");
                dt.Columns.Add("SalaryType");
                dt.Columns.Add("ReversalReason");
            }
            else if (key == "accrualsupload")
            {
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("Location");
                dt.Columns.Add("PayPeriod");
                dt.Columns.Add("HeadCount");
                dt.Columns.Add("CTC");
                dt.Columns.Add("AdditionalCTCAmount");
                dt.Columns.Add("ServiceCharge");
                dt.Columns.Add("AdditionalServiceCharge");
                dt.Columns.Add("SourcingFee");
                dt.Columns.Add("AbsorptionFee");
                dt.Columns.Add("OnboardingCharge");
                dt.Columns.Add("InedgeCharge");
                dt.Columns.Add("UpfrontCharge");
                dt.Columns.Add("Mode");
            }
            else if (key == "otherincomemapnamechanges")
            {
                dt.Columns.Add("COMPANY_CODE");
                dt.Columns.Add("EMPLOYEE_CODE");
                dt.Columns.Add("PAY_PERIOD");
                dt.Columns.Add("INPUT_NO");
                dt.Columns.Add("MAP_NAME");
                dt.Columns.Add("ACTION");
            }
            else if (key == "attendanceleaveupdate")
            {
                dt.Columns.Add("EMPLOYEE_CODE");
                dt.Columns.Add("PAY_PERIOD");
                dt.Columns.Add("LEAVE_OPENING_BALANCE");
                dt.Columns.Add("LEAVE_CREDIT");
                dt.Columns.Add("LEAVE_CLOSING_BALANCE");
                dt.Columns.Add("ACTUAL_LEAVE_CLOSING_BALANCE");
                dt.Columns.Add("LEAVE_AVAILED");
            }
            else if (key == "invoiceaxisdsa")
            {
                dt.Columns.Add("Invoice_Number");
                dt.Columns.Add("Personal_Loans");
                dt.Columns.Add("Gold_Loan");
                dt.Columns.Add("Education_Loan");
                dt.Columns.Add("Auto");
                dt.Columns.Add("CV_CE");
                dt.Columns.Add("Two_Wheeler");
                dt.Columns.Add("Home_Loan");
                dt.Columns.Add("LAP");
                dt.Columns.Add("ASHA");
                dt.Columns.Add("LAS");
                dt.Columns.Add("SBB");
                dt.Columns.Add("B2B_B2C_GOLD_LOAN");
                dt.Columns.Add("MSME");
                dt.Columns.Add("CLWF");
                dt.Columns.Add("Tractor_Loan");
                dt.Columns.Add("RENT");
                dt.Columns.Add("HOUSEKEEPING");
                dt.Columns.Add("SECURITY");
                dt.Columns.Add("ELECTRICITY");
                dt.Columns.Add("DEPRECIATION");
            }
            else if (key == "uanactivestatus")
            {
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("UanActiveStatus");
                dt.Columns.Add("UanActiveStatusRemarks");
            }
            else if (key == "invoicebayer")
            {
                dt.Columns.Add("Invoice_Number");
                dt.Columns.Add("PO_Number");
                dt.Columns.Add("PO_description");
                dt.Columns.Add("PO_line");
                dt.Columns.Add("Unit_Quantity");
                dt.Columns.Add("Unit_Cost");
                dt.Columns.Add("Total_Cost");
                dt.Columns.Add("GL");
                dt.Columns.Add("Cost_Center");
                dt.Columns.Add("Order_Number");
                dt.Columns.Add("CA_Name");
            }
            else if (key == "pfecrdataremoval")
            {
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("PayPeriod");
                dt.Columns.Add("EmployeeCode");
            }
            else if (key == "pannumberupload")
            {
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("PanNumberRemarks");
            }
            else if (key == "tiscactivationstatus")
            {
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("TiscActiveStatus");
                dt.Columns.Add("TiscActiveStatusRemarks");
            }
            else if (key == "esisubcode")
            {
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("EsiSubCode");
                dt.Columns.Add("EsiSubCodeName");
            }
            else if (key == "esiecrdataremoval")
            {
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("PayPeriod");
            }
            else if (key == "aadhaarauthenticatestatus")
            {
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("AadhaarAuthenticateStatus");
                dt.Columns.Add("Remarks");
            }
            else if (key == "submissiondateupdate")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("SubmissionDate");
            }
            else if (key == "invoicempversionone")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("Service");
                dt.Columns.Add("SAC");
                dt.Columns.Add("Units");
                dt.Columns.Add("Rate");
                dt.Columns.Add("Amount");
                dt.Columns.Add("TaxableValue");
                dt.Columns.Add("CGSTRate");
                dt.Columns.Add("CGSTAmount");
                dt.Columns.Add("SGSTRate");
                dt.Columns.Add("SGSTAmount");
                dt.Columns.Add("UTGSTRate");
                dt.Columns.Add("UTGSTAmount");
                dt.Columns.Add("IGSTRate");
                dt.Columns.Add("IGSTAmount");
                dt.Columns.Add("Total");
            }
            else if (key == "invoicempversiontwo")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("SAC");
                dt.Columns.Add("Service");
                dt.Columns.Add("NOP");
                dt.Columns.Add("Rate");
                dt.Columns.Add("PerDay");
                dt.Columns.Add("Present");
                dt.Columns.Add("Wage");
                dt.Columns.Add("EPF");
                dt.Columns.Add("ESI");
                dt.Columns.Add("Bonus");
                dt.Columns.Add("LWFR");
                dt.Columns.Add("INCENTIVE");
                dt.Columns.Add("ServiceCharge");
                dt.Columns.Add("TaxableAmount");
                dt.Columns.Add("CGSTRate");
                dt.Columns.Add("CGSTAmount");
                dt.Columns.Add("SGSTRate");
                dt.Columns.Add("SGSTAmount");
                dt.Columns.Add("UTGSTRate");
                dt.Columns.Add("UTGSTAmount");
                dt.Columns.Add("IGSTRate");
                dt.Columns.Add("IGSTAmount");
                dt.Columns.Add("Total");
            }
            else if (key == "invoicempversionthree")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("Designation");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Attendance");
                dt.Columns.Add("Rate");
                dt.Columns.Add("GrossAmount");
                dt.Columns.Add("Bonus");
                dt.Columns.Add("LWFR");
                dt.Columns.Add("INCENTIVE");
                dt.Columns.Add("PF");
                dt.Columns.Add("ESIC");
                dt.Columns.Add("ServiceCharge");
                dt.Columns.Add("TaxableCTC");
            }
            else if (key == "invoicempsalarydetail")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("Salary");
                dt.Columns.Add("Bonus");
                dt.Columns.Add("ESI");
                dt.Columns.Add("PF");
                dt.Columns.Add("ServiceCharge");
                dt.Columns.Add("LWFR");
                dt.Columns.Add("INCENTIVE");
            }
            else if (key == "pocultureupload")
            {
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("MapName");
                dt.Columns.Add("PurchaseRequestNo");
            }
            else if (key == "invoicevoltas")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("CallsClosed");
                dt.Columns.Add("ChargesPerCall");
            }
            else if (key == "invoicejohndeere")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("Discount_1");
                dt.Columns.Add("Discount_2");
            }
            else if (key == "qitsdepartment")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("Dept_Code");
                dt.Columns.Add("Dept_Name");
            }
            else if (key == "rejoineeupload")
            {
                dt.Columns.Add("OldCompanyCode");
                dt.Columns.Add("NewCompanyCode");
                dt.Columns.Add("NewDOJ");
                dt.Columns.Add("OldEmpID");
                dt.Columns.Add("Rejoinee");
                dt.Columns.Add("JoiningPayPeriod");
                dt.Columns.Add("GroupName");
                dt.Columns.Add("Department");
                dt.Columns.Add("Designation");
                dt.Columns.Add("PayCategory");
                dt.Columns.Add("CostCentre");
                dt.Columns.Add("Mapname");
                dt.Columns.Add("NewOmsid");
                dt.Columns.Add("EnityLocation");
            }
            else if (key == "ubrremarkupdate")
            {
                dt.Columns.Add("FinalStatus");
                dt.Columns.Add("ReasonForInvoicePending");
                dt.Columns.Add("ExpectedClosureDate");
                dt.Columns.Add("AxpertEmployeeID");
                dt.Columns.Add("PayPeriod");
            }
            else if (key == "pfedlicharges")
            {
                dt.Columns.Add("InvoiceNumber");
                dt.Columns.Add("PfEdliCharges");
            }
            else if (key == "wbsupdation")
            {
                dt.Columns.Add("EmployeeCode");
            }
            else if (key == "payregisteruploaddeletion")
            {
                dt.Columns.Add("Company_Code");
                dt.Columns.Add("Pay_Period");
                dt.Columns.Add("Employee_Code");
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("InvalidUploadType");
            }
          
            return dt;
        }

   }
}