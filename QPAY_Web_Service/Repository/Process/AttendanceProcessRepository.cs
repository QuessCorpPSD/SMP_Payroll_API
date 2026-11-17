using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Process;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using static QPay.IRepository.Repository.PurchaseOrder.PurchaseOrderRepository;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay_Web_Domain.Models.Timesheet;

namespace QPay.BAL.Repository.Process
{
    public class AttendanceProcessRepository : IAttendanceProcessRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public AttendanceProcessRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(SearchRequest searchRequest, string schema)
        {
            string xml = "<Main>" + "<Pay_Frequency_Id>" + searchRequest.searchxml.Pay_Frequency_Id + "</Pay_Frequency_Id>" + "<Company_id>" + searchRequest.searchxml.Company_id + "</Company_id><Resign_Status>" + searchRequest.searchxml.Resign_Status + "</Resign_Status><Emp_Code> " + searchRequest.searchxml.Emp_Code + " </Emp_Code><Process_Type>" + searchRequest.searchxml.Process_Type + "</Process_Type></Main>";

            var parameters = new Dictionary<string, object?>
            {
                ["@mode"] = searchRequest.mode,
                ["@value1"] = searchRequest.Value1,
                ["@value2"] = xml
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAttendanceDetailsData", parameters, 1500);
        }

        public async Task<DataSet> ExporttoExcel(ExporttoExcelRequest exporttoExcelRequest, string schema)
        {
            string xml = "<Main>" + "<Pay_Frequency_Id>" + exporttoExcelRequest.exporttoExcelxml.Pay_Frequency_Id + "</Pay_Frequency_Id>" + "<Company_id>" + exporttoExcelRequest.exporttoExcelxml.Company_id + "</Company_id></Main>";

            var parameters = new Dictionary<string, object?>
            {
                ["@mode"] = exporttoExcelRequest.mode,
                ["@value1"] = exporttoExcelRequest.Value1,
                ["@value2"] = xml
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Proc_GetAttendanceDetailsDataExportToExcel", parameters, 1500);
        }

        public async Task<AttendanceProcessResponse> ImportAttendnace(IFormFile file, [FromForm] string User, string schema)
        {
            AttendanceProcessResponse attendnaceProcessDetails = new AttendanceProcessResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Attendance_Process");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Attendance_Process_{datePrefix}{extension}";

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
                    attendnaceProcessDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return attendnaceProcessDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = schema + ".Proc_Upload_Attendance_NewUI";
                var parameters = new DynamicParameters();

                parameters.Add("@CreatedBy", User);
                parameters.Add("@XML_File", xmlInput.Replace("Sheet1", "Table"));

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("Import Successfully Done."))
                        {
                            attendnaceProcessDetails.response = message;
                        }
                        else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Uploaded faild due to"))
                        {
                            attendnaceProcessDetails.response = message;
                        }
                        else
                        {
                            attendnaceProcessDetails.response = "Failed to import.";
                            attendnaceProcessDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        attendnaceProcessDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    attendnaceProcessDetails.response = "Failed";
                }

            }
            else
            {
                attendnaceProcessDetails.response = "File not found";
            }
            return attendnaceProcessDetails;
        }

    }
}
