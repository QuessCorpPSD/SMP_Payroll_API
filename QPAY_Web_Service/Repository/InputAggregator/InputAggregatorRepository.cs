using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.InputAggregator;
using QPay.DAL.Repository;
using QPay.UI.Models.Aggregator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.InputAggregator
{
    public class InputAggregatorRepository : IInputAggregatorRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public InputAggregatorRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }



        public async Task<DataSet> QuessAttributeMaster()
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Quess_Attributes_Master", parameters, 1500);
        }

        public async Task<DataSet> ClientAttributes(int? companyId, int? siteid)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteid

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Client_Attributes_Master_SiteWise", parameters, 1500);
        }

        public async Task<DataSet> Search(int? companyId, int? siteid)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteid

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Quess_Client_Attributes_Mapping_detail_SiteWise", parameters, 1500);
        }

        public async Task<RequestResponse> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "ClientAttributes";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                //string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_Client_Attributes_SiteWise";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }

        public async Task<RequestResponse> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "AttributesMapping";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                // string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_Quess_Client_Attributes_mapping_SiteWise";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            var dataSet = new DataSet();

            using var workbook = new XLWorkbook(filePath);

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


        public async Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "InputAggregator";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + "/" + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                var paras = new Dictionary<string, object?>
                {
                    ["@companyId"] = CompanyId,
                    ["@siteId"] = SiteId

                };

                DataSet ListDs = this._dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Client_Billing_Attributes_SiteWise", paras, 1000);
                DataTable dt = new DataTable();
                dt = ListDs.Tables[0];
                List<string> selectedColumns = dt.AsEnumerable().Select(r => r.Field<string>("Template_Field_Name")).ToList();

                if (!selectedColumns.Any(x => string.IsNullOrWhiteSpace(x)))
                {
                    foreach (DataColumn column in dtToSerilize.Columns)
                    {
                        column.ColumnName = column.ColumnName.Replace(" ", "").Replace("00:00:00", "").Replace("/", "-");
                    }

                    DataTable result = new DataTable();
                    foreach (string col in selectedColumns)
                    {
                        result.Columns.Add(col, dtToSerilize.Columns[col].DataType);
                    }

                    foreach (DataRow row in dtToSerilize.Rows)
                    {
                        DataRow newRow = result.NewRow();
                        foreach (string col in selectedColumns)
                        {
                            newRow[col] = row[col];
                        }
                        result.Rows.Add(newRow);
                    }
                    DataSet dscolumns = new DataSet();
                    dscolumns.Tables.Add(result);
                    // Convert DataTable to XML
                    using var xmlWriter = new StringWriter();

                    dscolumns.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                    string xmlInput = xmlWriter.ToString();

                    xmlInput = xmlInput.Replace("'", "").Replace("_x0023_", "#");

                    string storeProcedure = @"Proc_Upload_InputAggregator_SiteWise";
                    var parameters = new DynamicParameters();
                    parameters.Add("@XML_File", xmlInput);
                    parameters.Add("@CreatedBy", CreatedBy);
                    parameters.Add("@Company_Id", CompanyId);
                    parameters.Add("@Site_Id", SiteId);
                    parameters.Add("@Pay_Period_Id", PayPeriodId);
                    var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        try
                        {
                            var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                            var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                            if (!string.IsNullOrWhiteSpace(message) &&
                                message.Contains("Successfully"))
                            {
                                poDetails.response = message;
                            }
                            else
                            {
                                poDetails.response = "Failed to import.";
                                poDetails.errors = res
                                    ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                    .ToList() ?? new List<string> { "Unknown error." };
                            }
                        }
                        catch (Exception ex)
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    else
                    {
                        poDetails.response = "Failed";
                        poDetails.errors = new List<string> { "Uploaded data not processed." };
                    }
                }
                else
                {
                    poDetails.response = "Failed to import.";
                    poDetails.errors = new List<string> { "Client mapping columns are incorrect." };
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }

        public List<string> GetSelectedColumns()
        {
            // Implement your logic to get the list of selected columns
            return new List<string>
            {
               "Remittance_DimCWSFinClientInvoice_InvoiceNumber0Grouping",
                "details_DimCWSFinAssignment_AssignmentNumber_WithOutLink",
                "details_DimCWSFinMiscFee_TransactionNumber",
                "details_WeekStarting_DimCWSFinDateView_ActualDate",
                "details_WeekEnding_DimCWSFinDateView_ActualDate",
                "details_FactCWSFinAssignmentRegister_InvoicedGrossFormatted"
            };
        }


        public async Task<DataSet> billableReport(int? companyId, int? payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
            }; // Sp_Search_billabledays_Upload
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Search_billabledays_Upload", parameters, 1500);
        }


        public async Task<RequestResponse> MISCUpload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "MISCUPLOAD";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "/").Replace("_x0029_", "").
                           Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "").Replace("_x005D_", "]").Replace("_x005B_", "[");

                xmlInput = xmlInput.Replace("[", "").Replace("]", "").Replace("'", "");



                string storeProcedure = "Proc_Upload_MISC_Billing_Aggregator_SiteWise";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                parameters.Add("@Company_Id", CompanyId);
                parameters.Add("@Site_Id", SiteId);
                parameters.Add("@Pay_Period_Id", PayPeriodId);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }


        public async Task<RequestResponse> OTRateUpload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "OTRateUpload";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "/").Replace("_x0029_", "").
                           Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "").Replace("_x005D_", "]").Replace("_x005B_", "[");

                xmlInput = xmlInput.Replace("[", "").Replace("]", "").Replace("'", "");



                string storeProcedure = "Proc_Upload_OT_Rate_Billing_Aggregator_SiteWise";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                parameters.Add("@Company_Id", CompanyId);
                parameters.Add("@Site_Id", SiteId);
                parameters.Add("@Pay_Period_Id", PayPeriodId);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }


        public async Task<DataSet> CompanyPaycode(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_company_paycode_adhoc_master", parameters, 1500);
        }

        public async Task<DataSet> MISCPaycodeSearch(int? companyId, int? siteid, string? Flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteid,
                ["@Action"] = Flag


            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_MISC_Paycode_Mapping_Detail_SiteWise", parameters, 1500);
        }
      
        public async Task<DataSet> MISCPaycodes(int? companyId, int siteid)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteid
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_MISC_Paycodes_Detail_SiteWise", parameters, 1500);
        }

        public async Task<DataSet> MISCPaycodeMapping(MISCPaycodeMappingRequest items)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = items.parentDetail.Company_Id,
                ["@Site_Id"] = items.parentDetail.SiteId,
                ["@FeeCode"] = items.parentDetail.FeeCode,
                ["@Pay_Code"] = items.parentDetail.Pay_Code,
                ["@Pay_Code_Id"] = items.parentDetail.Pay_Code_Id,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_CreateUpdate_MISC_Paycode_Mapping", parameters);
        }

        public async Task<DataSet> FinalSubmission(int? companyId, int? payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Search_billabledays_Upload_Auto_Integration", parameters, 1500);
        }

        public async Task<DataSet> ClientAttributesDelete(int? companyId,int? siteid, int? templateId, string CreatedBy)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteid,
                ["@Template_Id"] = templateId,
                ["@CreatedBy"] = CreatedBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_Billing_ClientWise_Attribute_master_delete_SiteWise", parameters, 1500);
        }

        public async Task<DataSet> ClientAttributesMaster(int? companyId, int? siteid)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteid
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_Billing_ClientWise_Attribute_master_SiteWise", parameters, 1500);
        }

        public async Task<DataSet> MappingAttributesDelete(int? companyId, int? templateId, int? Quess_Template_Field_Id, string CreatedBy)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Template_Id"] = templateId,
                ["@Quess_Template_Field_Id"] = Quess_Template_Field_Id,
                ["@CreatedBy"] = CreatedBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_Billing_Mapping_Attribute_delete", parameters, 1500);
        }


        public async Task<RequestResponse> ClientAttributesUploadOI(IFormFile file, [FromForm] string CreatedBy)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "ClientAttributesOI";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                //string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_Billing_Client_Attributes_OI_SiteWise";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }

        public async Task<RequestResponse> AttributesMappingUploadOI(IFormFile file, [FromForm] string CreatedBy)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "AttributesMappingOI";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                // string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_Billing_Quess_Client_Attributes_mapping_OI";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }


        public async Task<RequestResponse> UploadOI(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "InputAggregatorOI";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + "/" + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                var paras = new Dictionary<string, object?>
                {
                    ["@companyId"] = CompanyId
                };

                DataSet ListDs = this._dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Client_Billing_Attributes_OI", paras, 1000);
                DataTable dt = new DataTable();
                dt = ListDs.Tables[0];
                List<string> selectedColumns = dt.AsEnumerable().Select(r => r.Field<string>("Template_Field_Name")).ToList();

                if (!selectedColumns.Any(x => string.IsNullOrWhiteSpace(x)))
                {
                    foreach (DataColumn column in dtToSerilize.Columns)
                    {
                        column.ColumnName = column.ColumnName.Replace(" ", "").Replace("00:00:00", "").Replace("/", "-");
                    }

                    DataTable result = new DataTable();
                    foreach (string col in selectedColumns)
                    {
                        result.Columns.Add(col, dtToSerilize.Columns[col].DataType);
                    }

                    foreach (DataRow row in dtToSerilize.Rows)
                    {
                        DataRow newRow = result.NewRow();
                        foreach (string col in selectedColumns)
                        {
                            newRow[col] = row[col];
                        }
                        result.Rows.Add(newRow);
                    }
                    DataSet dscolumns = new DataSet();
                    dscolumns.Tables.Add(result);
                    // Convert DataTable to XML
                    using var xmlWriter = new StringWriter();

                    dscolumns.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                    string xmlInput = xmlWriter.ToString();

                    xmlInput = xmlInput.Replace("'", "");

                    string storeProcedure = @"Proc_Upload_Billing_InputAggregator_OI";
                    var parameters = new DynamicParameters();
                    parameters.Add("@XML_File", xmlInput);
                    parameters.Add("@CreatedBy", CreatedBy);
                    parameters.Add("@Company_Id", CompanyId);
                    parameters.Add("@Pay_Period_Id", PayPeriodId);
                    var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        try
                        {
                            var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                            var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                            if (!string.IsNullOrWhiteSpace(message) &&
                                message.Contains("Successfully"))
                            {
                                poDetails.response = message;
                            }
                            else
                            {
                                poDetails.response = "Failed to import.";
                                poDetails.errors = res
                                    ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                    .ToList() ?? new List<string> { "Unknown error." };
                            }
                        }
                        catch (Exception ex)
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    else
                    {
                        poDetails.response = "Failed";
                        poDetails.errors = new List<string> { "Uploaded data not processed." };
                    }
                }
                else
                {
                    poDetails.response = "Failed to import.";
                    poDetails.errors = new List<string> { "Client mapping columns are incorrect." };
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }



    }
}
