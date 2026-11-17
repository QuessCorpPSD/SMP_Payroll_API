using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using System.Data;
using static QPay.UI_Domain.Models.Master.VendorEmployee;

namespace QPay.IRepository.Repository.Master
{
    public class VendorEmployeeRepository : IVendorEmployeeRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public VendorEmployeeRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetVendorEmployee(string CompanyId, string SiteId, string EmployeeCode,
            string EActive)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = ToNullableInt(CompanyId),
                ["@SiteId"] = ToNullableInt(SiteId),
                ["@Employee_code"] = ToDbString(EmployeeCode),
                ["@EActive"] = (EActive == "Yes" || EActive == "1") ? 1
                   : (EActive == "No" || EActive == "0") ? 0
                   : (EActive == "All")? ToNullableInt("")
                   : ToNullableInt(EActive)
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_QZone_GetAllVendorEmployee", parameters, 1500);
        }

        public async Task<DataSet> GetVendorEmployeeTemplate()
        {
            var parameters = new Dictionary<string, object?>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_QZone_GetVenderTemplate", parameters, 1500);
        }

        public async Task<VendorEmployeeResponse> ImportVendorEmployee(IFormFile file, [FromForm] string User)
        {
            VendorEmployeeResponse vendorempDetails = new VendorEmployeeResponse();
            string sFileName = "";
            string fileNameToDelete = "";

            if (file != null && file.Length != 0)
            {
                var DirName = Path.Combine(_configuration["UAEPath"].ToString(), "Masters","VendorEmployee");
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
                    vendorempDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return vendorempDetails;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    vendorempDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return vendorempDetails;
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

                string storeProcedure = "Sp_QZone_ImportVendorEmployee";
                var parameters = new DynamicParameters();

                parameters.Add("@Xmldoc", xmlInput);
                parameters.Add("@Created_By", User);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Vendor Employee data uploaded successfully."))
                        {
                            vendorempDetails.response = "Vendor Employee data uploaded successfully.";
                        }
                        else
                        {
                            vendorempDetails.response = "Failed to import.";
                            vendorempDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        vendorempDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    vendorempDetails.response = "Failed";
                }

            }
            else
            {
                vendorempDetails.response = "File not found";
            }
            return vendorempDetails;
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

        public static object ToDbString(string? input)
        {
            return string.IsNullOrWhiteSpace(input) ? DBNull.Value : input!;
        }

        //private string BuildVendorEmployeeXml(VendorEmployeeRequest request)
        //{
        //    var sb = new StringBuilder();
        //    sb.Append("<NewDataSet>");

        //    foreach (var row in request.holidaymaster)
        //    {
        //        sb.Append("<VendorMaster>");
        //        sb.AppendFormat("<EMPLOYEE_CODE>{0}</EMPLOYEE_CODE>", row.EMPLOYEE_CODE);
        //        sb.AppendFormat("<EMPLOYEE_NAME>{0}</EMPLOYEE_NAME>", row.EMPLOYEE_NAME);
        //        sb.AppendFormat("<SiteName>{0}</SiteName>", row.SiteName);
        //        sb.AppendFormat("<State>{0}</State>", row.State);
        //        sb.AppendFormat("<Fromdate>{0}</Fromdate>", row.Fromdate);
        //        sb.AppendFormat("<Todate>{0}</Todate>", row.Todate);
        //        sb.AppendFormat("<HolidayDate>{0}</HolidayDate>", row.HolidayDate);
        //        sb.AppendFormat("<HolidayDescription>{0}</HolidayDescription>", row.HolidayDescription);
        //        sb.AppendFormat("<HolidayType>{0}</HolidayType>", row.HolidayType);
        //        sb.Append("</HolidayMaster>");
        //    }

        //    sb.Append("</NewDataSet>");
        //    return sb.ToString();
        //}

    }

}
