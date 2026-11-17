using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.InputAggregator;
using QPay.BAL.Repository;
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
    public class InputAggregatorAttendanceRepository : IInputAggregatorAttendanceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        //private readonly LoginRepository _logger;

        public InputAggregatorAttendanceRepository(DbRepository dbRepository, IConfiguration configuration)
            //,LoginRepository logger)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
            //this._logger = logger;
        }

        public async Task<DataSet> QuessLeaveMaster(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema+".SP_Quess_Leave_Master", parameters, 1500);
        }

        public async Task<DataSet> leaveTypeMaster(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".SP_Leave_Type_Master", parameters, 1500);
        }

        public async Task<DataSet> Createleavemapping(AttendanceAggregatorRequest items, string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Leave_mapping_detail_Id"] = items.parentDetail.LEAVE_MAPPING_DETAIL_ID,
                ["@Company_Id"] = items.parentDetail.COMPANY_ID,
                ["@Site_Id"] = items.parentDetail.SITEID,
                ["@Leave_Type_Id"] = items.parentDetail.LEAVE_TYPE_ID,
                ["@Leave_Treat_Id"] = items.parentDetail.LEAVE_TREAT_ID,
                ["@IsActive"] = items.parentDetail.ISACTIVE,
                ["@Header_Count"] = items.parentDetail.ATTENDANCE_TYPE,
                ["@Leave_Policy_Id"] = items.parentDetail.LEAVE_POLICY_ID,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
                ["@Buffer_date"] = items.parentDetail.BUFFER_DATE
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".SP_LEAVE_TYPE_MAPPING_DETAIL_SITEWISE", parameters);
        }

        public async Task<DataSet> SearchLeaveTypeMapping(int? companyId, int? siteid, string? Flag, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@companyId"] = companyId,
                ["@SiteId"] = siteid,
                ["@Action"] = Flag
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".SP_Leave_Type_Master_mapping_detail_Sitewise", parameters, 1500);
        }

        public async Task<DataSet> LeavePolicyMaster(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".SP_Leave_policy_Master", parameters, 1500);
        }



        public async Task<DataSet> Createleavetype(leaveTypeMasterRequest items, string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Leave_Type_Id"] = items.parentDetail.LEAVE_TYPE_ID,
                ["@Leave_Type_Name"] = items.parentDetail.LEAVE_TYPE_NAME,
                ["@Leave_Type_Description"] = items.parentDetail.LEAVE_TYPE_DESCRIPTION,
                ["@IsActive"] = items.parentDetail.ISACTIVE,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".SP_LEAVE_TYPE_DETAIL", parameters);
        }


        public async Task<DataSet> QuessAttendanceAttributeMaster(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                // ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_Get_Quess_Attendance_Attributes_Master", parameters, 1500);
        }

        public async Task<DataSet> ClientAttendanceAttribute(int? companyId, int? siteid, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@SiteId"] = siteid,

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_Get_Client_Attendance_Attributes_Master_SiteWise", parameters, 1500);
        }

        public async Task<DataSet> QuessAttendanceAttributeClient(int? companyId, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_Get_Quess_Attendance_Attributes_Client_Master", parameters, 1500);
        }

        public async Task<DataSet> ClientAttendanceAttributeMaster(int? companyId, int? siteid, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_Get_Client_Attendance_Attributes_SiteWise", parameters, 1500);
        }


        public async Task<RequestResponse> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy, string schema)
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
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = schema + ".Proc_Upload_Client_Attendance_Attributes_SiteWise";
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

        public async Task<RequestResponse> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy, string schema)
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
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = schema + ".Proc_Upload_Quess_Client_Attendance_Attributes_mapping_SiteWise";
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

        public async Task<DataSet> AttributesMappingCreate(AttributesMappingRequest items, string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = items.parentDetail.company_Id,
                ["@Template_Field_Id"] = items.parentDetail.Template_Field_Id,
                ["@Quess_Template_Field_Id"] = items.parentDetail.Quess_Template_Field_Id,
                ["@IsActive"] = items.parentDetail.IsActive,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".SP_Quess_Client_Attendance_Attributes_mapping", parameters);
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            var dataSet = new DataSet();
            try
            {
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
                                string columnName = cell.IsEmpty()
                                    ? $"Column{cell.Address.ColumnNumber}"
                                    : cell.GetValue<string>().Trim();

                                // Ensure unique column name
                                string newColumnName = columnName;
                                int count = 1;

                                while (dataTable.Columns.Contains(newColumnName))
                                {
                                    newColumnName = columnName + count;
                                    count++;
                                }

                                dataTable.Columns.Add(newColumnName);
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Excel file: " + ex.Message);
            }

            return dataSet;
        }

        public async Task<DataSet> Search(int? companyId, int? siteid, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteid,

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_Get_Quess_Client_Attendance_Attributes_Mapping_detail_SiteWise", parameters, 1500);
        }

        public async Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId, string schema)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                var parm = new Dictionary<string, object?>
                {
                    ["@companyId"] = CompanyId,
                    ["@siteId"] = SiteId
                };
                DataSet dsCheck = this._dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_Get_Validation_Check_SiteWise", parm, 1000);
                if (dsCheck.Tables[0].Rows.Count > 0)
                {
                    if (dsCheck.Tables[0].Rows[0]["STATUS_CODE"].ToString() == "200")
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

                        var parameters = new Dictionary<string, object?>
                        {
                            ["@companyId"] = CompanyId,
                            ["@siteId"] = SiteId

                        };

                        DataSet ListDs = this._dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Client_Attendance_Attributes_SiteWise", parameters, 1000);
                        DataTable dt = new DataTable();
                        dt = ListDs.Tables[0];
                        List<string> selectedColumns = dt.AsEnumerable().Select(r => r.Field<string>("Template_Field_Name")).ToList();

                        var duplicates = selectedColumns.Where(x => !string.IsNullOrWhiteSpace(x)).GroupBy(x => x.Trim())
                            .Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                        if (!duplicates.Any())
                        {
                            if (!selectedColumns.Any(x => string.IsNullOrWhiteSpace(x)))
                            {
                                foreach (DataColumn column in dtToSerilize.Columns)
                                {
                                    try
                                    {
                                        DateTime parsedDate;

                                        string[] formats = new[]
                                        {
    "dd-MM-yyyy HH:mm:ss",
    "dd-MM-yyyy",
    "yyyy-MM-dd HH:mm:ss",
    "yyyy-MM-dd",
    "dd/MM/yyyy HH:mm:ss",
    "dd/MM/yyyy",
    "MM/dd/yyyy HH:mm:ss",
    "MM/dd/yyyy",
    "yyyy/MM/dd HH:mm:ss",
    "yyyy/MM/dd",
    "yyyy-MM-ddTHH:mm:ss",
    "yyyy-MM-ddTHH:mm:ss.fff"
};

                                        if (DateTime.TryParseExact(
                                                column.ColumnName,
                                                formats,
                                                System.Globalization.CultureInfo.InvariantCulture,
                                                System.Globalization.DateTimeStyles.None,
                                                out parsedDate)
                                            || DateTime.TryParse(column.ColumnName, out parsedDate)) // fallback
                                        {
                                            column.ColumnName = "[" + parsedDate.ToString("dd") + "]";
                                        }
                                        else
                                        {
                                            column.ColumnName = "[" + column.ColumnName
                                                                        .Replace(" ", "")
                                                                        .Replace("/", "-") + "]";
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }

                                DataTable result = new DataTable();
                                foreach (string col in selectedColumns)
                                {
                                    if (dtToSerilize.Columns.Contains(col))
                                    {
                                        result.Columns.Add(col, dtToSerilize.Columns[col].DataType);
                                    }
                                }

                                // Copy rows
                                foreach (DataRow row in dtToSerilize.Rows)
                                {
                                    DataRow newRow = result.NewRow();

                                    foreach (string col in selectedColumns)
                                    {
                                        if (dtToSerilize.Columns.Contains(col))
                                        {
                                            newRow[col] = row[col];
                                        }
                                    }

                                    result.Rows.Add(newRow);
                                }

                                DataSet dscolumns = new DataSet();
                                dscolumns.Tables.Add(result);
                                for (int i = 0; i < dscolumns.Tables[0].Columns.Count; i++)
                                {
                                    dscolumns.Tables[0].Columns[i].ColumnName = dscolumns.Tables[0].Columns[i].ColumnName.Replace("[", "[A_");
                                }

                                // Convert DataTable to XML
                                using var xmlWriter = new StringWriter();

                                dscolumns.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                                // string xmlInput = xmlWriter.ToString();
                                string xmlInput = xmlWriter.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "/").Replace("_x0029_", "").
                                    Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "").Replace("_x005D_", "]").Replace("_x005B_", "[");

                                xmlInput = xmlInput.Replace("[", "").Replace("]", "").Replace("'", "");

                                string storeProcedure = schema + ".Proc_Upload_Attendance_InputAggregator_SiteWise";
                                var parameter = new DynamicParameters();
                                parameter.Add("@XML_File", xmlInput);
                                parameter.Add("@CreatedBy", CreatedBy);
                                parameter.Add("@Company_Id", CompanyId);
                                parameter.Add("@SiteId", SiteId);
                                parameter.Add("@Pay_Period_Id", PayPeriodId);
                                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
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
                                    catch
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
                            poDetails.response = "Failed to import.";
                            poDetails.errors = new List<string> { "Duplicate Template Field Names found Please check the date in client attribute : " +
                                         string.Join(", ", duplicates)};
                        }

                    }
                    else
                    {
                        poDetails.response = "Failed to import.";
                        poDetails.errors = new List<string> { "Client Attributes or Leave Master not configured. Please confiure before uploading client input file." };
                    }
                }
                else
                {

                    poDetails.response = "Failed to import.";
                    poDetails.errors = new List<string> { "Client Attributes or Leave Master not configured. Please confiure before uploading client input file." };
                }
            }
            else
            {
                poDetails.response = "File not found";
                poDetails.errors = new List<string> { "File not found" };
            }
            return poDetails;
        }

        public async Task<DataSet> AttendanceReport(int? companyId, int? payPeriodId, int? siteid, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Flag"] = 1,
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
                ["@Site_Id"] = siteid

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".USP_VIEW_TEMPLETE_INPUTATTENDANCE_AGGREGATOR_SiteWise", parameters, 1500);
        }

        public async Task<DataSet> ClientAttributesDelete(int? companyId, int? siteid, int? templateId, string CreatedBy, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@companyId"] = companyId,
                ["@siteId"] = siteid,
                ["@templateId"] = templateId,
                ["@CreatedBy"] = CreatedBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_Attendance_ClientWise_Attribute_master_delete_SiteWise", parameters, 1500);
        }

        public async Task<DataSet> billableReport(int? companyId, int? payPeriodId, int? siteid, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
                ["@Site_Id"] = siteid

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".USP_VIEW_TEMPLETE_INPUTBILLABLE_AGGREGATOR_SiteWise", parameters, 1500);
        }

        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        public async Task<DataSet> FinalSubmission(int? companyId, int? payPeriodId, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_Attendance_Aggregator_Upload_Auto_Integration", parameters, 1500);
        }


    }
}
