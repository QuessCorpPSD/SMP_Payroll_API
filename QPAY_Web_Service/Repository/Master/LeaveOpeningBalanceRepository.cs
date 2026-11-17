using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using System.Data;
using static QPay.UI_Domain.Models.Master.LeaveOpening;

namespace QPay.IRepository.Repository.Master
{
    public class LeaveOpeningBalanceRepository : ILeaveOpeningBalanceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public LeaveOpeningBalanceRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetLeaveOpeingBalance(string CompanyId, string Sitename)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_ID"] = ToNullableInt(CompanyId),
                ["@Sitename"] = Sitename
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_QZone_Display_leave", parameters, 1500);
        }

        public async Task<DataSet> GetLeaveOpeingBalanceTemplate(LeaveOpeningTemplateRequest leave)
        {
            if (leave != null) {
                var parameters = new Dictionary<string, object?>
                {
                    ["@Company_ID"] = ToNullableInt(leave.CompanyId),
                    ["@SiteName"] = ToNullableInt(leave.SiteId),
                    ["@Company_Code"] = leave.CompanyCode,
                    ["@Fromdate"] = leave.Fromdate,
                    ["@Todate"] = leave.Todate,

                };
                return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_Qzone_TemplateforLeaveDetails", parameters, 1500);
            }
            return new DataSet();
            
        }

        public async Task<LeaveOpeningResponse> ImportLeaveOpeingBalance(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string Fromdate, [FromForm] string Todate, [FromForm] string CreatedBy)
        {
            LeaveOpeningResponse leaveDetails = new LeaveOpeningResponse();
            string sFileName = "";
            string fileNameToDelete = "";

            if (file != null && file.Length != 0)
            {
                var DirName = Path.Combine(_configuration["UAEPath"].ToString(),"Masters", "LeaveOpeningBalance");
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                sFileName = file.FileName;
                sFileName = sFileName.Replace(" ", "");
                string Extension = Path.GetExtension(sFileName);

                string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(sFileName);
                string sfilewithExtension = FileNameWithoutExtension + Extension;
                fileNameToDelete = Guid.NewGuid().ToString() + sfilewithExtension;
                string excelPath = DirName + "\\" + fileNameToDelete;

                using (var stream = new FileStream(excelPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(excelPath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    leaveDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return leaveDetails;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    leaveDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return leaveDetails;
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

                string storeProcedure = "USP_QZone_Bulkupload_Leavedetails";
                var parameters = new DynamicParameters();

                parameters.Add("@company_ID", CompanyId);
                parameters.Add("@CreatedBy", CreatedBy);
                parameters.Add("@Fromdate", Fromdate);
                parameters.Add("@Todate", Todate);
                parameters.Add("@XML", xmlInput);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Leave Opening Balance data uploaded successfully."))
                        {
                            leaveDetails.response = "Leave Opening Balance data uploaded successfully.";
                        }
                        else
                        {
                            leaveDetails.response = "Failed to import.";
                            leaveDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        leaveDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    leaveDetails.response = "Failed";
                }

            }
            else
            {
                leaveDetails.response = "File not found";
            }
            return leaveDetails;
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

        public static int? ToNullableInt(string? input)
        {
            return int.TryParse(input, out int val) ? val : null;
        }
    }
}
