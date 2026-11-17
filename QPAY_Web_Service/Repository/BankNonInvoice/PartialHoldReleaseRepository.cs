using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class PartialHoldReleaseRepository : IPartialHoldReleaseRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public PartialHoldReleaseRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }
        public async Task<PartialEmpSalaryResponse> PartialHold(IFormFile file, string User)
        {
            PartialEmpSalaryResponse result = new PartialEmpSalaryResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    result.response = "File not found";
                    return result;
                }

                var dirName = Path.Combine(
                    _configuration["SalaryReleaseKey"].ToString(),
                    "BankNonInvoice",
                    "PartialHold"
                );

                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(dirName, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ✅ SAME AS CLIENT ADVANCE
                DataSet ds = new DataSet("NewDataSet");
                ds = ExcelToDataSet(filePath);
                ds.Tables[0].TableName = "Table";

                if (ds.Tables.Count == 0)
                {
                    result.response = "Excel sheet is empty.";
                    return result;
                }

                string xmlInput = "";
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                // ✅ SAME PARAM STYLE AS WORKING CODE
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", 3); // same as old system
                parameters.Add("@UploadType", "Partial Hold Salary");
                parameters.Add("@QZoneUserName", User);

                var res = await _dbRepository.GetItemsAsync(
                    "Proc_BankNonInvoice_GenericUpload", parameters);

                // ✅ SAME RESPONSE HANDLING STYLE
                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success", StringComparison.OrdinalIgnoreCase))
                    {
                        result.response = res;
                    }
                    else
                    {
                        result.response = "Failed to import.";
                        result.errors = res
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    result.response = "Failed";
                }
            }
            catch (Exception ex)
            {
                result.response = ex.Message;
            }

            return result;
        }

        public async Task<PartialEmpSalaryResponse> PartialHoldRelease(IFormFile file, string User)
        {
            PartialEmpSalaryResponse result = new PartialEmpSalaryResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    result.response = "File not found";
                    return result;
                }

                var dirName = Path.Combine(
                    _configuration["SalaryReleaseKey"].ToString(),
                    "BankNonInvoice",
                    "PartialHoldRelease"
                );

                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(dirName, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ✅ SAME AS CLIENT ADVANCE
                DataSet ds = new DataSet("NewDataSet");
                ds = ExcelToDataSet(filePath);
                ds.Tables[0].TableName = "Table";

                if (ds.Tables.Count == 0)
                {
                    result.response = "Excel sheet is empty.";
                    return result;
                }

                string xmlInput = "";
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                // ✅ SAME PARAM STYLE AS WORKING CODE
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", 3); // same as old system
                parameters.Add("@UploadType", "Partial Hold Salary Release");
                parameters.Add("@QZoneUserName", User);

                var res = await _dbRepository.GetItemsAsync(
                    "Proc_BankNonInvoice_GenericUpload", parameters);

                // ✅ SAME RESPONSE HANDLING STYLE
                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success", StringComparison.OrdinalIgnoreCase))
                    {
                        result.response = res;
                    }
                    else
                    {
                        result.response = "Failed to import.";
                        result.errors = res
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    result.response = "Failed";
                }
            }
            catch (Exception ex)
            {
                result.response = ex.Message;
            }

            return result;
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
