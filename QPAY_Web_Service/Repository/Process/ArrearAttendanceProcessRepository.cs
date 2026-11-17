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
using QPay.DAL.Repository;
using static QPay.IRepository.Repository.PurchaseOrder.PurchaseOrderRepository;
using static QPay.UI.Models.Process.AttendanceProcess;

namespace QPay.BAL.Repository.Process
{
    public class ArrearAttendanceProcessRepository : IArrearAttendanceProcessRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ArrearAttendanceProcessRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(SearchArrearRequest searchRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = searchRequest.Company_id,
                ["@Current_pay_sequence_id"] = searchRequest.Pay_Frequency_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_getArrearAttendanceData", parameters, 1500);
        }

        public async Task<AttendanceProcessResponse> ImportArrearAttendnace(IFormFile file, [FromForm] string User)
        {
            AttendanceProcessResponse attendnaceProcessDetails = new AttendanceProcessResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Arrear_Attendance_Process");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Arrear_Attendance_Process_{datePrefix}{extension}";

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

                string storeProcedure = "Proc_Upload_Arrear_Attendance_NewUI";
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

                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("Rows Import Successfully"))
                        {
                            attendnaceProcessDetails.response = message;
                        }
                        else if (!string.IsNullOrWhiteSpace(message) && message.Contains("No rows to Upload"))
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
