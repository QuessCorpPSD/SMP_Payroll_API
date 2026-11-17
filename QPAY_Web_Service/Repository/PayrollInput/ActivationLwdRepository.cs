using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.PayrollInput;
using QPay.DAL.Repository;
using static QPay.UI_Domain.Models.ActivationLwd;

namespace QPay.DAL.Repository.PayrollInput
{
    public class ActivationLwdRepository : IActivationLwdRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ActivationLwdRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            _configuration = configuration;
        }

        public async Task<object> GetEmployeeActivationLwd(string companyId, string flag)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@COMPANY_CODE", companyId);
            parameters.Add("@FLAG", flag);

            var res = await this._dbRepository.GetItemsAsync("PROC_QZONE_EXCEL_VIEW_ACTIVATION_LWD", parameters);

            if (string.IsNullOrEmpty(res)) return new { };

            return flag?.ToLower() switch
            {
                "1" => JsonConvert.DeserializeObject<List<Activation>>(res) ?? new List<Activation>(),
                "2" => JsonConvert.DeserializeObject<List<Lwd>>(res) ?? new List<Lwd>(),
                _ => new { message = "Invalid flag" }
            };
        }

        public async Task<ActivationResponse> UploadEmployeeActivation(IFormFile file, [FromForm] string User,
            [FromForm] string COMPANY_CODE, [FromForm] string FLAG)
        {
            ActivationResponse answerDetails = new ActivationResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Employee_Activation");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Activation_LWD_{COMPANY_CODE}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                dynamic employees;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);

                    var headerRow = worksheet.FirstRowUsed();
                    var columnMapping = new Dictionary<string, int>();

                    foreach (var cell in headerRow.Cells())
                    {
                        var header = cell.GetString().Trim().ToUpper();
                        columnMapping[header] = cell.Address.ColumnNumber;
                    }

                    var excel_data = ReadExcelWorksheetToDataTable(worksheet);
                    if (excel_data != null)
                    {
                        if (excel_data.Rows.Count > 0)
                        {
                            var expectedSet1 = new HashSet<string>
                            {
                                "COMPANY_CODE",
                                "EMPLOYEE_CODE",
                                "EMPLOYEE_NAME",
                                "DOJ",
                                "LAST_WORKING_DAY",
                                "REASON_OF_LEAVING",
                                "REMARKS",
                                "RELIEVING_LETTER"
                            };

                            var expectedSet2 = new HashSet<string>
                            {
                                "COMPANY_CODE",
                                "EMPLOYEE_CODE",
                                "EMPLOYEE_NAME",
                                "DOJ",
                                "LAST_WORKING_DAY",
                                "REASON_OF_LEAVING",
                                "RELIEVING_LETTER_YES_NO"
                            };

                            var actualColumns = excel_data?.Columns.Cast<DataColumn>()
        .Select(c => c.ColumnName)
        .ToHashSet(StringComparer.OrdinalIgnoreCase); // ignore case


                            if (actualColumns.SetEquals(expectedSet1))
                            {
                                FLAG = "1";
                            }
                            else if (actualColumns.SetEquals(expectedSet2))
                            {
                                FLAG = "2";
                            }
                            else
                            {
                                answerDetails.response = "Column headers are not matching";
                                return answerDetails;
                            }

                        }
                        else
                        {
                            answerDetails.response = "Record not found";
                            return answerDetails;
                        }
                    }
                    else
                    {
                        answerDetails.response = "Unable to extract the excel";
                        return answerDetails;
                    }

                    if (FLAG == "1")
                    {
                        employees = new List<ActivationEmployeelist>();
                    }
                    else if (FLAG == "2")
                    {
                        employees = new List<LWDEmployeelist>();
                    }
                    else
                    {
                        employees = new List<object>(); // or throw an exception
                    }

                    if (FLAG == "1")
                    {

                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            var emp = new ActivationEmployeelist
                            {
                                EMPLOYEE_CODE = row.Cell(columnMapping["EMPLOYEE_CODE"]).GetString().Trim(),
                                REMARKS = row.Cell(columnMapping["REMARKS"]).GetString().Trim()
                            };
                            employees.Add(emp);
                        }
                    }
                    else
                    {
                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            var emp1 = new LWDEmployeelist
                            {
                                Employee_Code = row.Cell(columnMapping["EMPLOYEE_CODE"]).GetString().Trim(),
                                First_Name = row.Cell(columnMapping["EMPLOYEE_NAME"]).GetString().Trim(),
                                DoJ = row.Cell(columnMapping["DOJ"]).GetString().Trim(),
                                Last_Working_Day = row.Cell(columnMapping["LAST_WORKING_DAY"]).GetString().Trim(),
                                Reason_Of_Leaving = row.Cell(columnMapping["REASON_OF_LEAVING"]).GetString().Trim(),
                                RELIEVING_LETTER_YES_NO = row.Cell(columnMapping["RELIEVING_LETTER_YES_NO"]).GetString().Trim()
                            };
                            employees.Add(emp1);
                        }
                    }
                }

                string xmlData = "";

                if (FLAG == "1")
                {
                    xmlData = GenerateXmlFromEmployees(employees);
                }
                else
                {
                    xmlData = GenerateXmlFromEmployeesLWD(employees);
                }

                string storeProcedure = "PROC_QZONE_EXCEL_UPLOAD_ACTIVATION_LWD";
                var parameters = new DynamicParameters();

                parameters.Add("@Xmldoc", xmlData);
                parameters.Add("@User", User);
                parameters.Add("@COMPANY_CODE", COMPANY_CODE);
                parameters.Add("@FLAG", FLAG);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Import Successfully Done", StringComparison.OrdinalIgnoreCase))
                        {
                            answerDetails.response = "Import Successfully Done.";
                        }
                        else
                        {
                            answerDetails.response = "Failed to import.";
                            answerDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        answerDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    answerDetails.response = "Failed";
                }

            }
            else
            {
                answerDetails.response = "File not found";
            }
            return answerDetails;
        }

        public string GenerateXmlFromEmployees(List<ActivationEmployeelist> employees)
        {
            var ds = new DataSet("NewDataSet");
            var dt = new DataTable("EmployeeMaster");
            dt.Columns.Add("EMPLOYEE_CODE", typeof(string));
            dt.Columns.Add("REMARKS", typeof(string));

            foreach (var emp in employees)
            {
                var row = dt.NewRow();
                row["EMPLOYEE_CODE"] = emp.EMPLOYEE_CODE;
                row["REMARKS"] = emp.REMARKS;
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                return sw.ToString();
            }
        }

        public async Task<ActivationResponse> UploadEmployeeLWD(IFormFile file, [FromForm] string User,
            [FromForm] string COMPANY_CODE, [FromForm] string FLAG)
        {
            ActivationResponse answerDetails = new ActivationResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Employee_LWD");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"LWD_{COMPANY_CODE}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                var employees = new List<LWDEmployeelist>();


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);

                    var headerRow = worksheet.FirstRowUsed();
                    var columnMapping = new Dictionary<string, int>();

                    foreach (var cell in headerRow.Cells())
                    {
                        var header = cell.GetString().Trim().ToUpper();
                        columnMapping[header] = cell.Address.ColumnNumber;
                    }

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var emp = new LWDEmployeelist
                        {
                            Employee_Code = row.Cell(columnMapping["EMPLOYEE_CODE"]).GetString().Trim(),
                            First_Name = row.Cell(columnMapping["EMPLOYEE_NAME"]).GetString().Trim(),
                            DoJ = row.Cell(columnMapping["DOJ"]).GetString().Trim(),
                            Last_Working_Day = row.Cell(columnMapping["LAST_WORKING_DAY"]).GetString().Trim(),
                            Reason_Of_Leaving = row.Cell(columnMapping["REASON_OF_LEAVING"]).GetString().Trim(),
                            RELIEVING_LETTER_YES_NO = row.Cell(columnMapping["RELIEVING_LETTER_YES_NO"]).GetString().Trim()                            
                        };
                        employees.Add(emp);
                    }
                }

                string xmlData = GenerateXmlFromEmployeesLWD(employees);

                string storeProcedure = "PROC_QZONE_EXCEL_UPLOAD_ACTIVATION_LWD";
                var parameters = new DynamicParameters();

                parameters.Add("@Xmldoc", xmlData);
                parameters.Add("@User", User);
                parameters.Add("@COMPANY_CODE", COMPANY_CODE);
                parameters.Add("@FLAG", FLAG);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Import Successfully Done"))
                        {
                            answerDetails.response = "Import Successfully Done.";
                        }
                        else
                        {
                                                        answerDetails.response = "Failed to import.";
                            answerDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        answerDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    answerDetails.response = "Failed";
                }

            }
            else
            {
                answerDetails.response = "File not found";
            }
            return answerDetails;
        }

        public string GenerateXmlFromEmployeesLWD(List<LWDEmployeelist> employees)
        {
            var ds = new DataSet("NewDataSet");
            var dt = new DataTable("EmployeeMaster");
            dt.Columns.Add("Employee_Code", typeof(string));
            dt.Columns.Add("First_Name", typeof(string));
            dt.Columns.Add("DoJ", typeof(string));
            dt.Columns.Add("Last_Working_Day", typeof(string));
            dt.Columns.Add("Reason_Of_Leaving", typeof(string));
            dt.Columns.Add("RELIEVING_LETTERYESNO", typeof(string));

            foreach (var emp in employees)
            {
                var row = dt.NewRow();
                row["Employee_Code"] = emp.Employee_Code;
                row["First_Name"] = emp.First_Name;
                row["DoJ"] = emp.DoJ;
                row["Last_Working_Day"] = emp.Last_Working_Day;
                row["Reason_Of_Leaving"] = emp.Reason_Of_Leaving;
                row["RELIEVING_LETTERYESNO"] = emp.RELIEVING_LETTER_YES_NO;
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                return sw.ToString();
            }
        }

        public static DataTable ReadExcelWorksheetToDataTable(IXLWorksheet worksheet, bool hasHeader = true)
        {
            var dataTable = new DataTable();
            var firstRowUsed = worksheet.FirstRowUsed();
            var row = firstRowUsed.RowUsed();

            int columnCount = worksheet.LastColumnUsed().ColumnNumber();

            // Add columns
            foreach (var cell in row.Cells(1, columnCount))
            {
                dataTable.Columns.Add(hasHeader ? cell.GetValue<string>() : $"Column {cell.Address.ColumnNumber}");
            }

            // Start reading after header if exists
            var firstDataRow = hasHeader ? row.RowBelow() : row;

            foreach (var dataRow in worksheet.Rows(firstDataRow.RowNumber(), worksheet.LastRowUsed().RowNumber()))
            {
                var data = new object[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    data[i] = dataRow.Cell(i + 1).Value;
                }
                dataTable.Rows.Add(data);
            }

            return dataTable;
        }
    }

}
