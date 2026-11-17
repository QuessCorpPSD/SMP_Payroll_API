using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using static QPay.UI_Domain.Models.Master.Holiday;

namespace QPay.IRepository.Repository.Master
{
    public class HolidayMasterRepository : IHolidayMasterRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public HolidayMasterRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetHolidayCompanywise(int CompanyId, int SiteId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@SiteId"] = SiteId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_QZONE_SelectHolidayComapnyWise", parameters, 1500);
        }

        public async Task<DataSet> GetHolidayTemplate()
        {
            var parameters = new Dictionary<string, object?>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Qzone_Holiday_DownLoadTemplate", parameters, 1500);
        }

        public async Task<HolidayMasterResponse> UploadHolidayMaster(IFormFile file, [FromForm] string User)
        {
            HolidayMasterResponse holidaymasterDetails = new HolidayMasterResponse();
            string sFileName = "";
            string fileNameToDelete = "";

            if (file != null && file.Length != 0)
            {
                var DirName = Path.Combine(_configuration["MasterPath"].ToString(), "HolidayMaster");
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
                    holidaymasterDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return holidaymasterDetails;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    holidaymasterDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return holidaymasterDetails;
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

                string storeProcedure = "proc_QZONE_bulk_EmployeeUpload";
                var parameters = new DynamicParameters();

                parameters.Add("@Xmldoc", xmlInput);
                parameters.Add("@Created_By", User);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Holiday Master data uploaded successfully."))
                        {
                            holidaymasterDetails.response = res;
                        }
                        else
                        {
                            holidaymasterDetails.response = "Failed to import.";
                            holidaymasterDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        holidaymasterDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    holidaymasterDetails.response = "Failed";
                }

            }
            else
            {
                holidaymasterDetails.response = "File not found";
            }
            return holidaymasterDetails;
        }

        public async Task<HolidayMasterResponse> SaveUpdateDeleteHolidayMaster([FromBody] HolidayMasterRequest request)
        {
            HolidayMasterResponse holidaymasterDetails = new HolidayMasterResponse();

            if (request == null || request.holidaymaster == null || !request.holidaymaster.Any())
            {
                holidaymasterDetails.response = "Invalid timesheet request.";
            }

            var xmlInput = BuildHolidayMasterXml(request);

            string storeProcedure = "Proc_QZONE_Create_Update_HolidayMaster";
            var parameters = new DynamicParameters();

            parameters.Add("@Xmldoc", xmlInput);
            parameters.Add("@Created_By", request.Created_By);
            parameters.Add("@Mode", request.Mode);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(res) && (res.Contains("Holiday Master data Added successfully.") || 
                        res.Contains("Holiday Master data Updated successfully.")||
                        res.Contains("Holiday Master data Deleted successfully.")))
                    {
                        holidaymasterDetails.response = res;
                    }
                    else
                    {
                        holidaymasterDetails.response = "Failed to "+ request.Mode + ".";
                        holidaymasterDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    holidaymasterDetails.response = "Error while processing response.";
                }
            }
            else
            {
                holidaymasterDetails.response = "Failed";
            }

            return holidaymasterDetails;
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

        private string BuildHolidayMasterXml(HolidayMasterRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<NewDataSet>");

            foreach (var row in request.holidaymaster)
            {
                sb.Append("<HolidayMaster>");
                sb.AppendFormat("<SrNo>{0}</SrNo>", row.SrNo);
                sb.AppendFormat("<CompanyCode>{0}</CompanyCode>", row.CompanyCode);
                sb.AppendFormat("<SiteName>{0}</SiteName>", row.SiteName);
                sb.AppendFormat("<State>{0}</State>", row.State);
                sb.AppendFormat("<Fromdate>{0}</Fromdate>", row.Fromdate);
                sb.AppendFormat("<Todate>{0}</Todate>", row.Todate);
                sb.AppendFormat("<HolidayDate>{0}</HolidayDate>", row.HolidayDate);
                sb.AppendFormat("<HolidayDescription>{0}</HolidayDescription>", row.HolidayDescription);
                sb.AppendFormat("<HolidayType>{0}</HolidayType>", row.HolidayType);
                sb.Append("</HolidayMaster>");
            }

            sb.Append("</NewDataSet>");
            return sb.ToString();
        }
    }
}
