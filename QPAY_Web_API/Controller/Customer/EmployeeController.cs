using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.Process;
using QPay.BAL.Repository.Process;
using QPay.UI.Customer;
using System.Data;
using static QPay.UI.Models.Process.AttendanceProcess;
using System.Text.RegularExpressions;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _IRepository;
        private readonly IConfiguration _configuration;
        public EmployeeController(IEmployeeRepository IRepository, IConfiguration configuration)
        {
            this._IRepository = IRepository;
            this._configuration = configuration;
        }

        [HttpGet, Route("SearchDetails/{companyId}/{Employee_code}")]
        public async Task<IActionResult> SearchDetails(int? companyId, string? Employee_code)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.SearchDetails(companyId, Employee_code, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetEmployeeBilltoRate/{companyId}")]
        public async Task<IActionResult> GetEmployeeBilltoRate(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GetEmployeeBilltoRate(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("ExportToExcel/{CompanyId}")]
        public async Task<IActionResult> ExportToExcel(int? CompanyId, string? EmployeeId, int? EActive)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.ExportToExcel(CompanyId, EmployeeId, EActive, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet, Route("GetCategory")]
        public async Task<IActionResult> GetCategory()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.GetCategory(schema);
            if (response != null)
            {

                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpGet, Route("GetWorkpass")]
        public async Task<IActionResult> GetWorkpass()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.GetWorkpass(schema);
            if (response != null)
            {

                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }


        [HttpGet, Route("Department/{companyId}")]
        public async Task<IActionResult> Department(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.Department(companyId,schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("Designation/{companyId}")]
        public async Task<IActionResult> Designation(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.Designation(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("BillingDesignation/{companyId}")]
        public async Task<IActionResult> BillingDesignation(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.BillingDesignation(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("Costcentermapping/{companyId}")]
        public async Task<IActionResult> Costcentermapping(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.Costcentermapping(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet, Route("Band/{companyId}")]
        public async Task<IActionResult> Band(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.Band(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet, Route("GetCostCenter/{companyId}")]
        public async Task<IActionResult> GetCostCenter(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GetCostCenter(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GroupMater/{companyId}")]
        public async Task<IActionResult> GroupMater(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GroupMater(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetAllPayPeriodByCompanyID/{companyId}")]
        public async Task<IActionResult> GetAllPayPeriodByCompanyID(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GetAllPayPeriodByCompanyID(companyId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("Bloodgroup")]
        public async Task<IActionResult> Bloodgroup()
        {
            var bloodgroups = new List<string>() { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            var payload = ResponseWrapManager.ResponseWrapper(bloodgroups, HttpContext);
            return Ok(payload);

        }

        [HttpGet, Route("MaritalStatus")]
        public async Task<IActionResult> MaritalStatus()
        {
            var bloodgroups = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            var payload = ResponseWrapManager.ResponseWrapper(bloodgroups, HttpContext);
            return Ok(payload);

        }


        [HttpGet, Route("SearchBank")]
        public async Task<IActionResult> SearchBank()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.SearchBank(schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("HiringStatus")]
        public async Task<IActionResult> HiringStatus()
        {
            var bloodgroups = new List<string>() { "Replacement", "Sourcing", "Transfer", "Absorption", "Client Referral", "Employee Referral" };
            var payload = ResponseWrapManager.ResponseWrapper(bloodgroups, HttpContext);
            return Ok(payload);

        }

        [HttpGet, Route("InsuranceStatus")]
        public async Task<IActionResult> InsuranceStatus()
        {
            var bloodgroups = new List<string>() { "Billable", "Deductable", "Non-billable", "Both" };
            var payload = ResponseWrapManager.ResponseWrapper(bloodgroups, HttpContext);
            return Ok(payload);

        }


        [HttpGet, Route("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GetRole(schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetEmploymentType")]
        public async Task<IActionResult> GetEmploymentType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GetEmploymentType(schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetReligion")]
        public async Task<IActionResult> GetReligion()
        {
            var bloodgroups = new List<string>() { "Christianity", "Islam", "Hinduism", "Buddhism", "NONE" };
            var payload = ResponseWrapManager.ResponseWrapper(bloodgroups, HttpContext);
            return Ok(payload);
        }


        [HttpPost]
        [Route("PostEmployeeUpload")]
        public async Task<IActionResult> PostEmployeeUpload(IFormFile file, [FromForm] string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "Employee";
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;

            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DataSet ds = new DataSet("NewDataSet");
            ds = ExcelToDataSetEmp(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return Ok("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();

            var response = await _IRepository.PostEmployeeUpload(xmlInput, userId, schema);

            return Ok(response);
        }

        [HttpPost]
        [Route("PostEmployeeSalaryUpload")]
        public async Task<IActionResult> PostEmployeeSalaryUpload(IFormFile file, [FromForm] string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "Employee_Salary";
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;

            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DataSet ds = new DataSet("NewDataSet");
            ds = ExcelToDataSetEmp(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return Ok("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _IRepository.PostEmployeeSalaryUpload(xmlInput, userId, schema);

            return Ok(response);
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
                            string rawName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            string columnName = Regex.Replace(rawName, @"[^a-zA-Z0-9_]", "");
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

        public static DataSet ExcelToDataSetEmp(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable("Table");
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string rawName = cell.IsEmpty()
                                ? $"Column{cell.Address.ColumnNumber}"
                                : cell.GetValue<string>();

                            string columnName = Regex.Replace(rawName, @"[^a-zA-Z0-9_]", "");

                            // Avoid duplicate column names
                            if (dataTable.Columns.Contains(columnName))
                                columnName += "_" + cell.Address.ColumnNumber;

                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                            .Select(cell =>
                            {
                                if (cell.IsEmpty())
                                    return string.Empty;

                                // ✅ Date handling
                                if (cell.DataType == XLDataType.DateTime)
                                {
                                    var date = cell.GetDateTime();
                                    return date.ToString("dd-MM-yyyy");
                                }

                                return cell.GetValue<string>();
                            })
                            .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] EmployeeRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Create(request, schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "200", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpPost]
        [Route("BankCreate")]
        public async Task<IActionResult> BankCreate([FromBody] EmployeeBankRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.BankCreate(request, schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "200", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpPost]
        [Route("InformationCreate")]
        public async Task<IActionResult> InformationCreate([FromBody] EmployeeInformationRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.InformationCreate(request, schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "200", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpPost]
        [Route("ContactCreate")]
        public async Task<IActionResult> ContactCreate([FromBody] EmployeeContactRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.ContactCreate(request, schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "200", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpPost]
        [Route("PersonalCreate")]
        public async Task<IActionResult> PersonalCreate([FromBody] EmployeePersonalRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.PersonalCreate(request, schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "200", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpPost]
        [Route("PreviousCreate")]
        public async Task<IActionResult> PreviousCreate([FromBody] EmployeePreviousRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.PreviousCreate(request,schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "200", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [Route("SearchSalary/{employeeId}")]
        public async Task<IActionResult> SearchSalary(int employeeId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.SearchSalary(employeeId, schema);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet, Route("GetLegalEntity")]
        public async Task<IActionResult> GetLegalEntity()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.GetLegalEntity(schema);

            return Ok(response);
        }


    }
}
