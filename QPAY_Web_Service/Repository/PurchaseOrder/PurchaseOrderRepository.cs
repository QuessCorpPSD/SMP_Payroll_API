using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QPay.BAL.IRepository.PurchaseOrder;
using QPay.DAL.Repository;
using QPay.UI.Models.SalaryReleaseInvoice;
using QPay.UI_Domain.Models.PurchaseOrder;
using System.Data;
using System.Text;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;


namespace QPay.IRepository.Repository.PurchaseOrder
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PurchaseOrderRepository> _logger;

        public PurchaseOrderRepository(DbRepository dbRepository, IConfiguration configuration,
            ILogger<PurchaseOrderRepository> logger)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
            _logger = logger;
        }
        public async Task<List<POCategoryUI>> GetPOCategory()
        {
            const string storedProcedure = "[dbo].[USP_PO_NEW_Category]";

            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<POCategoryUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<POCategoryUI>>(res);
                return list?.ToList() ?? new List<POCategoryUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<POCategoryUI>();
            }

        }
        public async Task<List<InvoiceTypeUI>> GetInvoiceType()
        {
            const string storedProcedure = "[dbo].[USP_PO_GETINVOICETYPE]";

            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<InvoiceTypeUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<InvoiceTypeUI>>(res);
                return list?.ToList() ?? new List<InvoiceTypeUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<InvoiceTypeUI>();
            }

        }

        public async Task<List<RAD_SP_INV_SHOWLIST_BYGROUPUI>> GetBillAndPricingType(string groupName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@GROUP_NAME", groupName);

            var result = await _dbRepository.GetItemsAsync<RAD_SP_INV_SHOWLIST_BYGROUPUI>(
                "RAD_SP_INV_SHOWLIST_BYGROUP",
                parameters
            );

            // Convert IEnumerable<T> → List<T>
            return result?.ToList() ?? new List<RAD_SP_INV_SHOWLIST_BYGROUPUI>();
        }

        public async Task<List<POUI>> POSearch(int companyId)
        {
            string storedProcedure = "USP_PONUMBER_Search";
            var parameter = new DynamicParameters();
            parameter.Add("@CompanyID", companyId);
            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<POUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<POUI>>(res);
                return list?.ToList() ?? new List<POUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<POUI>();
            }

        }

        public async Task<POSummaryUI> GetPOsAndCustomersAsync(int company_id, string pricingType, string PONumber, string flag)
        {

            // var multi =await _dbRepository.QueryMultiAsync("[dbo].[USP_GetPOsAndCustomers]", null);
            var parameter = new DynamicParameters();
            parameter.Add("@Company_id", company_id);
            parameter.Add("@Group_Detail_Id", "0");
            parameter.Add("@PricingType", pricingType);
            parameter.Add("@PONumber", PONumber);
            parameter.Add("@flag", flag);
            var result = await _dbRepository.QueryMultipleAsync(
                                    "[dbo].[USP_POCREATE_Search]",
                                    new[] { typeof(List<POSearchUI>), typeof(CompanyPOSummaryUI) }
                                    , parameter);


            var pos = (List<POSearchUI>)result[0];
            var sum = (List<CompanyPOSummaryUI>)result[1];
            POSummaryUI summaryUI = new POSummaryUI();
            summaryUI.pOSearches = pos;
            summaryUI.summary = sum.FirstOrDefault() ?? new CompanyPOSummaryUI();

            return (summaryUI);
        }
        public DataSet EmployeePOSearch(int companyId, string poNumber, string employeeId, string status, string pricingType)
        {
            DataSet ds = this._dbRepository.EmployeePOSearch(companyId, poNumber, employeeId, status, pricingType);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }

        }

        public async Task<List<BulkUploadResponse>> BulkPoUpload(string actionType, string xmlData, string employeeCode)
        {
            var storeProcedure = "Proc_Main_PO_Upload" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", actionType);
            parameter.Add("@XmlData", xmlData);
            parameter.Add("@CreatedBy", employeeCode);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BulkUploadResponse>(); // return empty object if no result
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<BulkUploadResponse>>(res);
                return list?.ToList() ?? new List<BulkUploadResponse>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<BulkUploadResponse>();
            }
        }
        public async Task<POQuantityUI> GetPOQuantityByCompanyId(string startDate, string endDate, int poQuantityType, int companyId)
        {
            const string storedProcedure = "[dbo].[USP_PO_Quantatity_CalcuLATION]";

            var parameters = new DynamicParameters();
            parameters.Add("@StartDate", startDate);
            parameters.Add("@ExpDate", endDate);
            parameters.Add("@QUANTITYTYPE", poQuantityType);
            parameters.Add("@SITEID", companyId);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameters);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new POQuantityUI(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<POQuantityUI>>(res);
                return list?.FirstOrDefault() ?? new POQuantityUI();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new POQuantityUI();
            }
        }


        public DataSet MainPoSearch(int companyId, string PricingType, string PONumber)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = companyId,
                ["@PricingType"] = PricingType,
                ["@PONumber"] = PONumber
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_MAIN_PO_Search", parameters, 1500);
        }

        public async Task<PoResponse> CreateMainPo(CreateMainPoRequest request)
        {
            PoResponse PoDetails = new PoResponse();

            if (request == null)
            {
                PoDetails.response = "Invalid Po Request";
                return PoDetails;
            }

            string storeProcedure = "USP_MAIN_PO_CREATION";
            var parameters = new DynamicParameters();

            parameters.Add("@Company_ID", request.Company_ID);
            parameters.Add("@PO_ID", request.Po_ID);
            parameters.Add("@PODate", request.PODate);
            parameters.Add("@StartDate", request.StartDate);
            parameters.Add("@ExpDate", request.ExpDate);
            parameters.Add("@PoNumber", request.PoNumber);
            parameters.Add("@PricingType", request.PricingType);
            parameters.Add("@POValue", request.POValue);
            parameters.Add("@CurrencyType", request.CurrencyType);
            parameters.Add("@POQuantutyType", request.POQuantutyType);
            parameters.Add("@POQuantuty", request.POQuantuty);
            parameters.Add("@InteExternal", request.InteExternal);

            string _poNumber = request.PoNumber;
            string _poNumberO = _poNumber.Replace('/', '_');
            string _poNumber0 = _poNumberO.Replace('&', '_');
            string _poNumber1 = _poNumber0.Trim();
            var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "PO", "MainPO", _poNumber1);
            string docpath = Path.Combine(uploadsFolder, request.DocPath);

            parameters.Add("@DocPath", docpath);
            parameters.Add("@Extention", request.Extention);
            parameters.Add("@ExtendedStartDate", request.ExtendedStartDate);
            parameters.Add("@ExtendedEndDate", request.ExtendedEndDate);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@BillingType", request.BillingType);
            parameters.Add("@ACTION", request.ACTION);
            parameters.Add("@PO_CategoryID", request.Po_CategoryID);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("SuccessFully Updated The New PO Number Is:"))
                    {
                        PoDetails.response = message;
                    }
                    else if (message.Contains("PO Number is Existing For the Company Code"))
                    {
                        PoDetails.response = message;
                    }
                    else if (message.Contains("SuccessFully Updated The Ext. PO Number Is:"))
                    {
                        PoDetails.response = message;
                    }
                    else if (message.Contains("For this PO Not tagged to Employee"))
                    {
                        PoDetails.response = message;
                    }
                    else if (message.Contains("SuccessFully Updated The PO Number Is:"))
                    {
                        PoDetails.response = message;
                    }
                    else if (message.Contains("The selected PO Data is under approval process, PO Creation and Ext. Can be done only after 12 Hours."))
                    {
                        PoDetails.response = message;
                    }
                    else
                    {
                        PoDetails.response = "Failed to import.";
                        PoDetails.errors = !string.IsNullOrWhiteSpace(message)
                            ? message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                            : new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    PoDetails.response = "Error while processing response.";
                }
            }
            else
            {
                PoDetails.response = "Failed";
            }
            return PoDetails;
        }

        public class ResponseModel
        {
            public string? Result { get; set; }
            public string? Error_Message { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public async Task<PoResponse> FileUploadMainPo(IFormFile file, [FromForm] string PoNumber,
          [FromForm] string File_Name, [FromForm] string File_Path, [FromForm] string CreatedBy)
        {
            PoResponse poDetails = new PoResponse();

            if (file != null && file.Length != 0)
            {

                string _poNumber = PoNumber;
                string _poNumberO = _poNumber.Replace('/', '_');
                string _poNumber0 = _poNumberO.Replace('&', '_');
                string _poNumber1 = _poNumber0.Trim();

                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "PO", "MainPO", _poNumber1);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);


                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = File_Name;

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var sb = new StringBuilder();
                sb.Append("<Table>");
                sb.Append("<File>");
                sb.AppendFormat("<PoNumber>{0}</PoNumber>", PoNumber);
                sb.AppendFormat("<File_Name>{0}</File_Name>", newFileName);
                sb.AppendFormat("<File_Path>{0}</File_Path>", uploadsFolder);
                sb.Append("</File>");
                sb.Append("</Table>");

                string storeProcedure = "USP_PO_MAIN_File_Upload";
                var parameters = new DynamicParameters();

                parameters.Add("@XML", sb.ToString());
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Record(s) Inserted Successfully!"))
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

        public DataSet GetPoCreateTemplate(string flag, string CreatedBy)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = flag,
                ["@CreatedBy"] = CreatedBy
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Main_PO_Upload", parameters, 1500);
        }

        public async Task<PoResponse> BulkPOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy)
        {
            PoResponse poDetails = new PoResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "PO", "MainPO", "BulkPO");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"BulkPO_{CreatedBy}_{datePrefix}{extension}";

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
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }

                DataSet dscolumns = new DataSet();
                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        // Replace PODate, StartDate, EndDate values with formatted string
                        row["PODate"] = DateTime.Parse(row["PODate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        row["StartDate"] = DateTime.Parse(row["StartDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        row["EndDate"] = DateTime.Parse(row["EndDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");

                        if (table.Columns.Contains("ExtentionEndDate") &&
    row["ExtentionEndDate"] != DBNull.Value &&
    !string.IsNullOrWhiteSpace(row["ExtentionEndDate"].ToString()))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(row["ExtentionEndDate"].ToString(), out dt))
                            {
                                row["ExtentionEndDate"] = dt.ToString("dd-MM-yyyy HH:mm:ss");
                            }
                        }
                    }
                }

                using var xmlWriter = new StringWriter();
                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();

                string storeProcedure = "Proc_Main_PO_Upload";
                var parameters = new DynamicParameters();

                parameters.Add("@Action", flag);
                parameters.Add("@XmlData", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {

                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var item = resultList?.FirstOrDefault();
                        var message = item?.Result ?? item?.Error_Message;

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
                    catch (Exception ex)
                    {
                        poDetails.response = ex.Message;
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

        public async Task<List<POStatus>> GetPOStatus()
        {
            const string storedProcedure = "[dbo].[GET_PO_STATUS]";

            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<POStatus>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<POStatus>>(res);
                return list?.ToList() ?? new List<POStatus>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<POStatus>();
            }

        }

        public DataSet EmployeePoView(int companyId, string poNumber, string clientEmployeeID, string status,
           string pricingType)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@COMPANY_ID"] = companyId,
                ["@PONumber"] = poNumber,
                ["@ClientEmployeeID"] = clientEmployeeID,
                ["@Status"] = status,
                ["@PricingType"] = ""
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_MAIN_PO_VIEW_EMPLOYEE_NEWUI", parameters, 1500);
        }

        public DataSet EmployeePoViewApprove(int companyId, string poNumber, string pricingType, string employeeId,
            string status, string employeeListId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@COMPANY_ID"] = companyId,
                ["@PONUMBER"] = poNumber,
                ["@Pricingtype"] = pricingType,
                ["@EMPLOYEEID"] = employeeId,
                ["@STATUS"] = status,
                ["@EmployeeListID"] = employeeListId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_MAIN_PO_VIEW_APPROVE", parameters, 1500);
        }

        public DataSet EmployeePoViewEmpdtls(int companyId, string poNumber, string pricingType, string employeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@COMPANY_ID"] = companyId,
                ["@CLIENTEMPID"] = employeeId,
                ["@PRICINGTYPE"] = pricingType,
                ["@PONUMBER"] = poNumber
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_MAIN_PO_VIEW_EMPDTLS", parameters, 1500);
        }

        public DataSet EmployeePoViewApproveNew(int companyId, string poNumber, string pricingType, string employeeId,
            string status)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@COMPANY_ID"] = companyId,
                ["@SITEID"] = "",
                ["@PONUMBER"] = poNumber,
                ["@Pricingtype"] = pricingType,
                ["@EMPLOYEEID"] = employeeId,
                ["@STATUS"] = status
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_PO_VIEW_APPROVE_NEW", parameters, 1500);
        }

        public async Task<PoResponse> EmployeePoNewCreation(EmployeePoNewCreationModel request)
        {
            PoResponse PoDetails = new PoResponse();

            if (request == null)
            {
                PoDetails.response = "Invalid Po Request";
                return PoDetails;
            }

            string storeProcedure = "USP_PO_EMPLOYEE_NEWCREATION";
            var parameters = new DynamicParameters();

            parameters.Add("@POID", request.POID);
            parameters.Add("@Company_ID", request.Company_ID);
            parameters.Add("@Group_Detail_Id", request.Group_Detail_Id);
            parameters.Add("@PoNumber", request.PoNumber);
            parameters.Add("@EmployeeListID", request.EmployeeListID);
            parameters.Add("@ResourceType", request.ResourceType);
            parameters.Add("@StartDate", request.StartDate);
            parameters.Add("@ExpDate", request.ExpDate);
            parameters.Add("@Duration", request.Duration);
            parameters.Add("@ItemType", request.ItemType);
            parameters.Add("@Balanceamount", request.Balanceamount);
            parameters.Add("@EmployeeID", request.EmployeeID);
            parameters.Add("@EmployeeName", request.EmployeeName);
            parameters.Add("@ClientEmpNo", request.ClientEmpNo);
            parameters.Add("@OfferPOValue", request.OfferPOValue);
            parameters.Add("@QuantityType", request.QuantityType);
            parameters.Add("@Quantity", request.Quantity);
            parameters.Add("@CTC", request.CTC);
            parameters.Add("@ServiceCharge", request.ServiceCharge);
            parameters.Add("@FixedRate", request.FixedRate);
            parameters.Add("@UnitPrice", request.UnitPrice);
            parameters.Add("@MonthlyRate", request.MonthlyRate);
            parameters.Add("@Totalresourcevalue", request.Totalresourcevalue);
            parameters.Add("@EmployeePOAttachment", request.EmployeePOAttachment);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@Flag", request.Flag);
            parameters.Add("@PricingType", request.PricingType);
            parameters.Add("@ChkFlag", request.ChkFlag);
            parameters.Add("@ExtStartDate", request.ExtStartDate);
            parameters.Add("@ExtEndDate", request.ExtEndDate);
            parameters.Add("@ExtDuration", request.ExtDuration);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("EMPID Already Tagged , Please Do the Extention (OR) Revised the Same PO"))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Successfuly updated the Employee PO"))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message)
                        && message.Contains("The selected PO Data is approval process, PO Creation and Ext. Can be done after 12 Hours."))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Not Enough PO Balance."))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("SuccessFully Updated!.."))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Successfuly updated the Revised Employee PO"))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Successfuly updated the Extention Employee PO"))
                    {
                        PoDetails.response = message;
                    }
                    else
                    {
                        PoDetails.response = "Failed to import.";
                        PoDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    PoDetails.response = "Error while processing response.";
                }
            }
            else
            {
                PoDetails.response = "Failed";
            }
            return PoDetails;
        }

        public async Task<PoResponse> EmployeePoEdit(EmployeePoEditModel request)
        {
            PoResponse PoDetails = new PoResponse();

            if (request == null)
            {
                PoDetails.response = "Invalid Po Request";
                return PoDetails;
            }

            string storeProcedure = "USP_EMP_PO_EDIT";
            var parameters = new DynamicParameters();

            parameters.Add("@StartDate", request.StartDate);
            parameters.Add("@ExpDate", request.ExpDate);
            parameters.Add("@Duration", request.Duration);
            parameters.Add("@ItemType", request.ItemType);
            parameters.Add("@Balanceamount", request.Balanceamount);
            parameters.Add("@QuantityType", request.QuantityType);
            parameters.Add("@Quantity", request.Quantity);
            parameters.Add("@CTC", request.CTC);
            parameters.Add("@ServiceCharge", request.ServiceCharge);
            parameters.Add("@FixedRate", request.FixedRate);
            parameters.Add("@MonthlyRate", request.MonthlyRate);
            parameters.Add("@Totalresourcevalue", request.Totalresourcevalue);
            parameters.Add("@EmployeeListID", request.EmployeeListID);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@Flag", request.Flag);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(res) && res.Contains("SuccessFully Updated!.."))
                    {
                        PoDetails.response = res;
                    }
                    else if (!string.IsNullOrWhiteSpace(res) && res.Contains("The selected PO Data is approval process, PO Creation and Ext. Can be done after 12 Hours."))
                    {
                        PoDetails.response = res;
                    }
                    else if (!string.IsNullOrWhiteSpace(res) && res.Contains("Not Enough PO Balance"))
                    {
                        PoDetails.response = res;
                    }
                    else
                    {
                        PoDetails.response = "Failed to import.";
                        PoDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    PoDetails.response = "Error while processing response.";
                }
            }
            else
            {
                PoDetails.response = "Failed";
            }
            return PoDetails;
        }

        public async Task<PoResponse> EmployeePoInsertRevised(EmployeePoInsertModel request)
        {
            PoResponse PoDetails = new PoResponse();

            if (request == null)
            {
                PoDetails.response = "Invalid Po Request";
                return PoDetails;
            }

            string storeProcedure = "USP_PO_INSERT_EMPLOYEEPO";
            var parameters = new DynamicParameters();

            parameters.Add("@POID", request.POID);
            parameters.Add("@Company_ID", request.Company_ID);
            parameters.Add("@Group_Detail_Id", request.Group_Detail_Id);
            parameters.Add("@PoNumber", request.PoNumber);
            parameters.Add("@EmployeeListID", request.EmployeeListID);
            parameters.Add("@ResourceType", request.ResourceType);
            parameters.Add("@StartDate", request.StartDate);
            parameters.Add("@ExpDate", request.ExpDate);
            parameters.Add("@Duration", request.Duration);
            parameters.Add("@ItemType", request.ItemType);
            parameters.Add("@Balanceamount", request.Balanceamount);
            parameters.Add("@EmployeeID", request.EmployeeID);
            parameters.Add("@EmployeeName", request.EmployeeName);
            parameters.Add("@ClientEmpNo", request.ClientEmpNo);
            parameters.Add("@OfferPOValue", request.OfferPOValue);
            parameters.Add("@QuantityType", request.QuantityType);
            parameters.Add("@Quantity", request.Quantity);
            parameters.Add("@CTC", request.CTC);
            parameters.Add("@ServiceCharge", request.ServiceCharge);
            parameters.Add("@FixedRate", request.FixedRate);
            parameters.Add("@UnitPrice", request.UnitPrice);
            parameters.Add("@MonthlyRate", request.MonthlyRate);
            parameters.Add("@Totalresourcevalue", request.Totalresourcevalue);
            parameters.Add("@EmployeePOAttachment", request.EmployeePOAttachment);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@Flag", request.Flag);
            parameters.Add("@PricingType", request.PricingType);
            parameters.Add("@ChkFlag", request.ChkFlag);
            parameters.Add("@ExtStartDate", request.ExtStartDate);
            parameters.Add("@ExtEndDate", request.ExtEndDate);
            parameters.Add("@ExtDuration", request.ExtDuration);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(res) && res.Contains("Successfuly updated the Revised & Extention Employee PO"))
                    {
                        PoDetails.response = res;
                    }
                    if (!string.IsNullOrWhiteSpace(res) && res.Contains("Successfuly updated the Extention Employee PO"))
                    {
                        PoDetails.response = res;
                    }
                    else if (!string.IsNullOrWhiteSpace(res) && res.Contains("Successfuly updated the Employee PO"))
                    {
                        PoDetails.response = res;
                    }
                    else if (!string.IsNullOrWhiteSpace(res) && res.Contains("Successfuly updated the Employee PO','Employee data not updated. CompanyId / SiteID Mismatch"))
                    {
                        PoDetails.response = res;
                    }
                    else if (!string.IsNullOrWhiteSpace(res) && res.Contains("Successfuly updated the Revised Employee PO"))
                    {
                        PoDetails.response = res;
                    }
                    else if (!string.IsNullOrWhiteSpace(res) && res.Contains("The selected PO Data is approval process, PO Creation and Ext. Can be done after 12 Hours."))
                    {
                        PoDetails.response = res;
                    }
                    else if (!string.IsNullOrWhiteSpace(res) && res.Contains("Not Enough PO Balance"))
                    {
                        PoDetails.response = res;
                    }
                    else
                    {
                        PoDetails.response = "Failed to import.";
                        PoDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    PoDetails.response = "Error while processing response.";
                }
            }
            else
            {
                PoDetails.response = "Failed";
            }
            return PoDetails;
        }

        public DataSet EmployeePoCreateTemplate(string flag, string CreatedBy)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = flag,
                ["@CreatedBy"] = CreatedBy
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_EmployeePO_Upload", parameters, 1500);
        }

        public async Task<PoResponse> BulkEmployeePOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy)
        {
            PoResponse poDetails = new PoResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "PO", "EmployeePO", "BulkEmployeePO");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"BulkPO_{CreatedBy}_{datePrefix}{extension}";

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
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }

                DataSet dscolumns = new DataSet();
                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        // Replace StartDate, EndDate values with formatted string
                        row["StartDate"] = DateTime.Parse(row["StartDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        row["EndDate"] = DateTime.Parse(row["EndDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");

                        if (table.Columns.Contains("ExtnEndDate") &&
    row["ExtnEndDate"] != DBNull.Value &&
    !string.IsNullOrWhiteSpace(row["ExtnEndDate"].ToString()))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(row["ExtnEndDate"].ToString(), out dt))
                            {
                                row["ExtnEndDate"] = dt.ToString("dd-MM-yyyy HH:mm:ss");
                            }
                        }

                        if (table.Columns.Contains("RevisionEndDate") &&
    row["RevisionEndDate"] != DBNull.Value &&
    !string.IsNullOrWhiteSpace(row["RevisionEndDate"].ToString()))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(row["RevisionEndDate"].ToString(), out dt))
                            {
                                row["RevisionEndDate"] = dt.ToString("dd-MM-yyyy HH:mm:ss");
                            }
                        }
                    }
                }

                using var xmlWriter = new StringWriter();
                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();

                string storeProcedure = "Proc_EmployeePO_Upload";
                var parameters = new DynamicParameters();

                parameters.Add("@Action", flag);
                parameters.Add("@XmlData", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {

                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

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

        public async Task<List<ItemTypeUI>> GetPoItemType()
        {
            const string storedProcedure = "[dbo].[USP_PO_GETITEMTYPE]";

            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<ItemTypeUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<ItemTypeUI>>(res);
                return list?.ToList() ?? new List<ItemTypeUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<ItemTypeUI>();
            }

        }


        public async Task<POValueUI> GetPOValue(string company_Id, string site_Id, string quantity_type, string quantity, string po_Rate)
        {
            const string storedProcedure = "[dbo].[USP_PO_EMPVALUE_QZONE_NEWUI]";

            var parameters = new DynamicParameters();
            parameters.Add("@COMPANY_ID", company_Id);
            parameters.Add("@SITEID", site_Id);
            parameters.Add("@QUANTITYTYPE", quantity_type);
            parameters.Add("@QUANTITY", quantity);
            parameters.Add("@FIXEDRATE", po_Rate);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameters);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new POValueUI(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<POValueUI>>(res);
                return list?.FirstOrDefault() ?? new POValueUI();
            }
            catch (JsonException ex)
            {
                return new POValueUI();
            }
        }

        public async Task<PoResponse> FileUploadEmployeePo(IFormFile file, [FromForm] string PoNumber,
          [FromForm] string File_Name, [FromForm] string EmployeeId, [FromForm] string CreatedBy)
        {
            PoResponse poDetails = new PoResponse();

            if (file != null && file.Length != 0)
            {

                string _poNumber = PoNumber;
                string _poNumberO = _poNumber.Replace('/', '_');
                string _poNumber0 = _poNumberO.Replace('&', '_');
                string _poNumber1 = _poNumber0.Trim();

                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "PO", "EmployeePO", _poNumber1);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);


                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = File_Name;

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var sb = new StringBuilder();
                sb.Append("<Table>");
                sb.Append("<File>");
                sb.AppendFormat("<PO_ID>{0}</PO_ID>", PoNumber);
                sb.AppendFormat("<EmployeeID>{0}</EmployeeID>", EmployeeId);
                sb.AppendFormat("<File_Name>{0}</File_Name>", File_Name);
                sb.AppendFormat("<File_Path>{0}</File_Path>", uploadsFolder);
                sb.Append("</File>");
                sb.Append("</Table>");

                string storeProcedure = "USP_PO_EMPLOYEE_File_Upload";
                var parameters = new DynamicParameters();

                parameters.Add("@XML", sb.ToString());
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Record(s) Inserted Successfully!"))
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


        public async Task<PoResponse> EmployeePoDelete(string EmployeeListID, string CreatedBy)
        {
            PoResponse poDetails = new PoResponse();

            string storeProcedure = "USP_PO_EMPLOYEE_File_Upload";
            var parameters = new DynamicParameters();

            parameters.Add("@EmployeeListID", EmployeeListID);
            parameters.Add("@CreatedBy", CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("SuccessFully Deleted!.."))
                    {
                        poDetails.response = message;
                    }
                    else
                    {
                        poDetails.response = "Failed";
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


            return poDetails;
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
