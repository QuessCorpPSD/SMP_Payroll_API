using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.Invoice;

namespace QPay.BAL.Repository.Invoice
{
    public class GSTInvoiceRepository : IGSTInvoiceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _config;

        public GSTInvoiceRepository(DbRepository dbRepository, IConfiguration config)
        {
            this._dbRepository = dbRepository;
            this._config = config;
        }

        public async Task<List<GstInvoiceGrid>> GetGSTInvoice(int userId)
        {

            string storeProcedure = "[dbo].[Proc_ManageGstInvoice_newUI]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "Get");
            parameter.Add("@UserId", userId);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<GstInvoiceGrid>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<GstInvoiceGrid>>(res);
                return list?.ToList() ?? new List<GstInvoiceGrid>();
            }
            catch (JsonException ex)
            {
                return new List<GstInvoiceGrid>();
            }


        }
        public DataSet GetInvoiceData(int invoiceId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "GetInvoiceHtml",
                ["@Company_Id"] = 0,
                ["@InvoiceId"] = invoiceId,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetInvoiceDetails", parameters, 1500);
        }

        public async Task<string> PostCancelReject(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Cancelled");
            parameters.Add("@Status", "Cancelled");
            parameters.Add("@XmlData", xmlString);
            parameters.Add("@UserId", userId);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> Create(GstInvoiceCreateRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", request.Action);
            parameters.Add("@Created_Mode", request.Created_Mode);
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Invoice_Id", request.Invoice_Id, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@Invoice_Number", request.Invoice_Number);
            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@Cost_Center_Mapping_Id", request.Cost_Center_Mapping_Id);
            parameters.Add("@City_Id", request.City_Id);
            parameters.Add("@Financial_Year_Id", request.Financial_Year_Id);
            parameters.Add("@Pay_Period_Id", request.Pay_Period_Id);
            parameters.Add("@Invoice_Type_Id", request.Invoice_Type_Id);
            parameters.Add("@Invoice_Date", request.Invoice_Date);
            parameters.Add("@Invoice_Due_Date", request.Invoice_Due_Date);
            parameters.Add("@Particulars", request.Particulars);
            parameters.Add("@Amount", request.Amount);
            parameters.Add("@StateId", request.StateId);
            parameters.Add("@InvoicingStateId", request.InvoicingStateId);
            parameters.Add("@CGST_Percentage", request.CGST_Percentage);
            parameters.Add("@SGST_Percentage", request.SGST_Percentage);
            parameters.Add("@UTGST_Percentage", request.UTGST_Percentage);
            parameters.Add("@IGST_Percentage", request.IGST_Percentage);
            parameters.Add("@Client_PO", request.Client_PO);
            parameters.Add("@Purchase_Order_Id", request.Purchase_Order_Id);
            parameters.Add("@Input_Date", request.Input_Date);
            parameters.Add("@Output_Date", request.Output_Date);
            parameters.Add("@Service_Charge", request.Service_Charge);
            parameters.Add("@Service_Charge_Amount", request.Service_Charge_Amount);
            parameters.Add("@Sourcing_Fee", request.Sourcing_Fee);
            parameters.Add("@Sourcing_Fee_Amount", request.Sourcing_Fee_Amount);
            parameters.Add("@No_Of_Employees", request.No_Of_Employees);
            parameters.Add("@Absorption_Fee", request.Absorption_Fee);
            parameters.Add("@Absorption_Amt", request.Absorption_Amt);
            parameters.Add("@CTC_Amt_Adjusted", request.CTC_Amt_Adjusted);
            parameters.Add("@CTC_Amt_NorP", request.CTC_Amt_NorP);
            parameters.Add("@CTC_Adj_Note", request.CTC_Adj_Note);
            parameters.Add("@Net_Amt_Adjusted", request.Net_Amt_Adjusted);
            parameters.Add("@Net_Amt_NorP", request.Net_Amt_NorP);
            parameters.Add("@Net_Adj_Note", request.Net_Adj_Note);
            parameters.Add("@Invoice_Culture_Id", request.Invoice_Culture_Id);
            parameters.Add("@Invoice_Culture_RefNo", request.Invoice_Culture_RefNo);
            parameters.Add("@Input_No", request.Input_No);
            parameters.Add("@Employee_ESI", request.Employee_ESI);
            parameters.Add("@Employer_ESI", request.Employer_ESI);
            parameters.Add("@Employee_PF", request.Employee_PF);
            parameters.Add("@Employer_PF", request.Employer_PF);
            parameters.Add("@Mobile_Recovery_Amount", request.Mobile_Recovery_Amount);
            parameters.Add("@Personal_Loan_Amount", request.Personal_Loan_Amount);
            parameters.Add("@Other_Deduction_Amount", request.Other_Deduction_Amount);
            parameters.Add("@WO_Number", request.WO_Number);
            parameters.Add("@Pl_Id_No", request.Pl_Id_No);
            parameters.Add("@Employee_Name", request.Employee_Name);
            parameters.Add("@Markup", request.Markup);
            parameters.Add("@Gri_Msp", request.Gri_Msp);
            parameters.Add("@DO_Number", request.DO_Number);
            parameters.Add("@Remarks", request.Remarks);
            parameters.Add("@Status", request.Status);
            parameters.Add("@IsActive", request.IsActive);
            parameters.Add("@WO_Date", request.WO_Date);
            parameters.Add("@InvoiceNotes", request.InvoiceNotes);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@CreatedOn", request.CreatedOn);
            parameters.Add("@ModifiedBy", request.ModifiedBy);
            parameters.Add("@ModifiedOn", request.ModifiedOn);
            parameters.Add("@Discrepancy_By", request.Discrepancy_By);
            parameters.Add("@Discrepancy_Reason", request.Discrepancy_Reason);
            parameters.Add("@Onboarding_Charge", request.Onboarding_Charge);
            parameters.Add("@Group_Detail_Id", request.Group_Detail_Id);
            parameters.Add("@TaxableAmount1", request.TaxableAmount1);
            parameters.Add("@TaxableAmount1_Note", request.TaxableAmount1_Note);
            parameters.Add("@TaxableAmount2", request.TaxableAmount2);
            parameters.Add("@TaxableAmount2_Note", request.TaxableAmount2_Note);
            parameters.Add("@TaxableAmount3", request.TaxableAmount3);
            parameters.Add("@TaxableAmount3_Note", request.TaxableAmount3_Note);
            parameters.Add("@NonTaxableAmount1", request.NonTaxableAmount1);
            parameters.Add("@NonTaxableAmount1_Note", request.NonTaxableAmount1_Note);
            parameters.Add("@NonTaxableAmount2", request.NonTaxableAmount2);
            parameters.Add("@NonTaxableAmount2_Note", request.NonTaxableAmount2_Note);
            parameters.Add("@NonTaxableAmount3", request.NonTaxableAmount3);
            parameters.Add("@NonTaxableAmount3_Note", request.NonTaxableAmount3_Note);
            parameters.Add("@Billable_Type_Id", request.Billable_Type_Id);
            parameters.Add("@ProvisionalInvoiceNumber", request.ProvisionalInvoiceNumber);
            parameters.Add("@Compliance_Fee", request.Compliance_Fee);
            parameters.Add("@Compliance_Fee_Amount", request.Compliance_Fee_Amount);
            parameters.Add("@Ctc_Deduction_Type_Id", request.Ctc_Deduction_Type_Id);
            parameters.Add("@Net_Deduction_Type_Id", request.Net_Deduction_Type_Id);
            parameters.Add("@GratuityInterest", request.Gratuityinterest);
            parameters.Add("@InsuranceAmount", request.InsuranceAmount);
            parameters.Add("@NewInvoiceNumber", request.NewInvoiceNumber);
            parameters.Add("@BGVBL", request.BGVBL);
            parameters.Add("@ASTFEE", request.ASTFEE);
            parameters.Add("@DISCT1", request.DISCT1);
            parameters.Add("@DISCT2", request.DISCT2);
            parameters.Add("@IDCARD", request.IDCARD);
            parameters.Add("@EMAIL", request.EMAIL);
            parameters.Add("@REGFEE", request.REGFEE);
            parameters.Add("@TRNFEE", request.TRNFEE);
            parameters.Add("@GGDBT", request.GGDBT);
            parameters.Add("@PPEKIT", request.PPEKIT);
            parameters.Add("@VMSFEE", request.VMSFEE);
            parameters.Add("@CALCRG", request.CALCRG);
            parameters.Add("@CALRT", request.CALRT);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "Failed";
        }


        //public async Task<GstInvoiceCreateResponse> Create(GstInvoiceCreateRequest request)
        //{
        //    GstInvoiceCreateResponse invoiceDetails = new GstInvoiceCreateResponse();

        //    string result = await this._dbRepository.ExecuteGstInvoiceAsync(request);

        //    if (!string.IsNullOrWhiteSpace(result))
        //    {
        //        if (result.Contains("InvoiceId"))
        //        {
        //            // Success
        //            invoiceDetails.response = "Success";

        //            // Extract InvoiceId + Data from the result JSON
        //            var parsed = JsonConvert.DeserializeObject<GstInvoiceCreateResponse>(result);

        //            invoiceDetails.InvoiceId = parsed.InvoiceId;
        //            invoiceDetails.Data = parsed.Data;
        //        }
        //        else
        //        {
        //            // Failure
        //            invoiceDetails.response = "Failed";
        //        }
        //    }
        //    else
        //    {
        //        invoiceDetails.response = "Failed";
        //    }

        //    return invoiceDetails;

        //}

        public async Task<List<UI.Models.Invoice.InvoiceTypeUI>> GetGSTInvoiceType()
        {
            string storeProcedure = "[dbo].[SP_GET_INVOICE_TYPE]" ?? "";
            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<UI.Models.Invoice.InvoiceTypeUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<UI.Models.Invoice.InvoiceTypeUI>>(res);
                return list?.ToList() ?? new List<UI.Models.Invoice.InvoiceTypeUI>();
            }
            catch (JsonException ex)
            {
                return new List<UI.Models.Invoice.InvoiceTypeUI>();
            }
        }

        public async Task<List<BillingTypeUI>> GetGSTBillableType()
        {
            string storeProcedure = "[dbo].[SP_GET_Billable_Type]" ?? "";
            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BillingTypeUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<BillingTypeUI>>(res);
                return list?.ToList() ?? new List<BillingTypeUI>();
            }
            catch (JsonException ex)
            {
                return new List<BillingTypeUI>();
            }
        }

        public async Task<List<CtcDeductionUI>> GetGSTCtcDeductionType()
        {
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetCtcDeductionType");

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<CtcDeductionUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<CtcDeductionUI>>(res);
                return list?.ToList() ?? new List<CtcDeductionUI>();
            }
            catch (JsonException ex)
            {
                return new List<CtcDeductionUI>();
            }
        }

        public async Task<List<NewDeductionUI>> GetGSTNetDeductionType()
        {
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetNetDeductionType");

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<NewDeductionUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<NewDeductionUI>>(res);
                return list?.ToList() ?? new List<NewDeductionUI>();
            }
            catch (JsonException ex)
            {
                return new List<NewDeductionUI>();
            }
        }

        public async Task<List<GSTPercentageUI>> GetGSTPercentage()
        {
            string storeProcedure = "[dbo].[Proc_ManageGstInvoice_NewUI]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetGstRates");
            parameter.Add("@UserId", "1075");

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<GSTPercentageUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<GSTPercentageUI>>(res);
                return list?.ToList() ?? new List<GSTPercentageUI>();
            }
            catch (JsonException ex)
            {
                return new List<GSTPercentageUI>();
            }


        }

        public async Task<List<AttributeUI>> GetAllAttribute(AttributeUI attributeUI)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", attributeUI.id);
            parameter.Add("@AttributeName", attributeUI.AttributeName);
            parameter.Add("@Isactive", attributeUI.IsActive);
            parameter.Add("@CreatedBy", attributeUI.CreatedBy);
            parameter.Add("@ActionType", attributeUI.ActionType);
            parameter.Add("@CompanyId", attributeUI.CompanyId);

            var res = await _dbRepository.GetItemsAsync("SP_tbl_Attributes_AddUpdate", parameter);

            if (res != null)
            {
                var attribute = JsonConvert.DeserializeObject<List<AttributeUI>>(res);
                return attribute;
            }
            else
            {
                return new List<AttributeUI>();
            }
        }

        public async Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
          [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_config["ClaimDocPath"].ToString(), "Invoice", "Attributes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Attributes_{CompanyId}_{payperiodId}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    invoiceDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return invoiceDetails;
                }

                DataTable dtToSerialize = ds.Tables[0];

                if (!dtToSerialize.Columns.Contains("Company_Id"))
                    dtToSerialize.Columns.Add("Company_Id", typeof(string));

                if (!dtToSerialize.Columns.Contains("PayPeriod_Id"))
                    dtToSerialize.Columns.Add("PayPeriod_Id", typeof(int));

                // Add extra columns that SQL expects
                if (!dtToSerialize.Columns.Contains("Narration"))
                    dtToSerialize.Columns.Add("Narration", typeof(string));

                if (!dtToSerialize.Columns.Contains("PO_Number"))
                    dtToSerialize.Columns.Add("PO_Number", typeof(string));

                if (!dtToSerialize.Columns.Contains("GL_Code"))
                    dtToSerialize.Columns.Add("GL_Code", typeof(string));

                if (!dtToSerialize.Columns.Contains("Cost_Center_Name"))
                    dtToSerialize.Columns.Add("Cost_Center_Name", typeof(string));

                if (!dtToSerialize.Columns.Contains("Client_SPOC_Name"))
                    dtToSerialize.Columns.Add("Client_SPOC_Name", typeof(string));

                if (!dtToSerialize.Columns.Contains("Work_Order_Number"))
                    dtToSerialize.Columns.Add("Work_Order_Number", typeof(string));

                foreach (DataRow row in dtToSerialize.Rows)
                {
                    row["Company_Id"] = CompanyId;   // or actual PayPeriod from UI
                    row["PayPeriod_Id"] = payperiodId;
                }

                foreach (DataRow row in dtToSerialize.Rows)
                {
                    foreach (DataColumn col in dtToSerialize.Columns)
                    {
                        if (row.IsNull(col))
                            row[col] = string.Empty; // replace DBNull with empty string
                    }
                }


                // Convert to XML
                using var xmlWriter = new StringWriter();
                dtToSerialize.TableName = "Table";  // Required for SQL XQuery
                DataSet xmlDS = new DataSet("NewDataSet");
                xmlDS.Tables.Add(dtToSerialize.Copy());

                xmlDS.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string storeProcedure = "Proc_Upload_GSTInvoice_Attributes";
                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully.", StringComparison.OrdinalIgnoreCase))
                        {
                            invoiceDetails.response = message;
                        }
                        else
                        {
                            invoiceDetails.response = "Failed to import.";
                            invoiceDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        invoiceDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    invoiceDetails.response = "Failed";
                }
            }
            else
            {
                invoiceDetails.response = "File not found";
            }
            return invoiceDetails;
        }

        public class ResponseModel
        {
            public string Result { get; set; }
            public string Error_Message { get; set; }
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

    }
}
