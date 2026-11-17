using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.BAL.IRepository.SalaryAdvance;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.SalaryAdvance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.IRepository.Repository.PurchaseOrder.PurchaseOrderRepository;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace QPay.BAL.Repository.SalaryAdvance
{
    public class ApproveRepository : IApproveRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ApproveRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? CompanyId, int? PayPeriod_Id, int? employeeid)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = CompanyId,
                ["@PayPeriod_Id"] = PayPeriod_Id,
                ["@Employee_Id"] = employeeid
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_SearchSalarydvanceApprove", parameters);
        }

        public async Task<DataSet> GetTemplate()
        {
            var parameters = new Dictionary<string, object>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_salaryadvance_Approve_Template", parameters);
        }

        public async Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy,
          [FromForm] string CompanyCode, [FromForm] int Payperiod_id, [FromForm] string Payperiod)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string[] Code = CompanyCode.ToString().Split('(');
                DirName += Code[0].ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }
                DirName += "\\" + Payperiod.ToString();
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

                string storeProcedure = "Proc_UploadSalaryAdvanceApprove";
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
