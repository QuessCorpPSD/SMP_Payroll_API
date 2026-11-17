using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.PayrollInput;
using System.ComponentModel.Design;
using System.Xml;
using System.Xml.Linq;
using ClosedXML.Excel;
using System.Xml.Linq;          
using System.Data;
using static Azure.Core.HttpHeader;
using System.Windows.Input;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using QPay.IRepository.iRepository.Common;
using QPay.UI.Models;

namespace Qzone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingRepository _ionboarding;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public OnboardingController(
            IOnboardingRepository iOnboarding, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._ionboarding = iOnboarding;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllOnboardingDetails/{companyCode}/{payPeriod}")]
        public async Task<IActionResult> GetAllOnboardingDetails(string companyCode, string? payPeriod)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _ionboarding.GetAllOnboardingDetails(companyCode, payPeriod, schema);

            return Ok(response);
        }

        [HttpPost, Route("GetNewJoineeTemplate")]
        public async Task<IActionResult> GetNewJoineeTemplate([FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] int flag, [FromForm] int mapNameId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (flag == 1)
            {
                List<PayperiodDD> payperiod = new List<PayperiodDD>();
                payperiod = _ionboarding.GetCurrentPayperiod(companyId, schema);
                if (payperiod != null && payperiod.Any())
                {
                    payPeriodId = payperiod[0].Payfrequencyid;
                }
                else
                {
                    return Ok("No pay period found for the given company ID.");
                }
            }
            DataSet ds = await _ionboarding.GetNewJoineeTemplate(companyId, payPeriodId, flag, mapNameId, schema);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], GetSheetName(i));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        if (flag == 1)
                        {
                            fileResponse.FileName = "NewJoinee_Template" + fileName;
                        }
                        else if (flag == 2)
                        {
                            fileResponse.FileName = "Attendance_Template" + fileName;
                        }
                        else if (flag == 5)
                        {
                            fileResponse.FileName = "OneTimeInput_Template" + fileName;
                        }
                            fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }
                }
            }
            else
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "Failure",
                    Data = "",
                    Error = ""
                };
                return Ok(response);
            }

        }
        private string GetSheetName(int i)
        {
            string sheetName = string.Empty;
            switch (i)
            {
                case 0:
                    sheetName = "EMPLOYEE";
                    break;
                case 1:
                    sheetName = "ADHOC";
                    break;

                default:
                    sheetName = "";
                    return sheetName;
            }
            return sheetName;

        }

        [HttpGet, Route("MoveToQpay/{offerIdJson}/{companyId}/{payPeriod}/{payPeriodId}/{userId}")]
        public async Task<IActionResult> MoveToQpay(string offerIdJson, int companyId, string payPeriod, int payPeriodId, string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            string[] harbourIds = JsonConvert.DeserializeObject<string[]>(offerIdJson);

            // Create XML structure
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement newDataSet = xmlDoc.CreateElement("NewDataSet");

            foreach (var id in harbourIds)
            {
                XmlElement offerDetails = xmlDoc.CreateElement("OFFER_DETAILS");
                XmlElement harbourId = xmlDoc.CreateElement("HARBOUR_ID");
                harbourId.InnerText = id;
                offerDetails.AppendChild(harbourId);
                newDataSet.AppendChild(offerDetails);
            }

            xmlDoc.AppendChild(newDataSet);

            string xmlString = xmlDoc.OuterXml;
            var response = await _ionboarding.MoveToQpay(xmlString, companyId, payPeriod, payPeriodId, userId, schema);

            return Ok(response);
        }

        [HttpGet, Route("PostValidateOfferId/{offerIds}")]
        public async Task<IActionResult> PostValidateOfferId(string offerIds)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            string[] harbourIds = JsonConvert.DeserializeObject<string[]>(offerIds);

            // Create XML structure
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement newDataSet = xmlDoc.CreateElement("XmlDS");

            foreach (var id in harbourIds)
            {
                XmlElement offerDetails = xmlDoc.CreateElement("Table");
                XmlElement harbourId = xmlDoc.CreateElement("OfferID");
                harbourId.InnerText = id;
                offerDetails.AppendChild(harbourId);
                newDataSet.AppendChild(offerDetails);
            }
            
            xmlDoc.AppendChild(newDataSet);

            string xmlString = xmlDoc.OuterXml;
            var response = await _ionboarding.PostValidateOfferId(xmlString, schema);
            return Ok(response);
        }

        [HttpGet, Route("PostRollbackOfferId/{offerIds}/{userId}")]
        public async Task<IActionResult> PostRollbackOfferId(string offerIds, string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            string[] harbourIds = JsonConvert.DeserializeObject<string[]>(offerIds);

            // Create XML structure
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement newDataSet = xmlDoc.CreateElement("XmlDS");

            foreach (var id in harbourIds)
            {
                XmlElement offerDetails = xmlDoc.CreateElement("Table");
                XmlElement harbourId = xmlDoc.CreateElement("OfferID");
                harbourId.InnerText = id;
                offerDetails.AppendChild(harbourId);
                newDataSet.AppendChild(offerDetails);
            }

            xmlDoc.AppendChild(newDataSet);

            string xmlString = xmlDoc.OuterXml;
            var response = await _ionboarding.PostRollbackOfferId(xmlString, userId, schema);

            return Ok(response);
        }
        [HttpPost]
        [Route("PostNewJoineeData")]
        public async Task<IActionResult> PostNewJoineeData(IFormFile file, [FromForm] string companyCode,
            [FromForm] int companyId, [FromForm] string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            List<PayperiodDD> payperiod = new List<PayperiodDD>();
            payperiod = _ionboarding.GetCurrentPayperiod(companyId, schema);
            int payPeriodId;
            string payPeriod = string.Empty;
            if (payperiod != null && payperiod.Any())
            {
                payPeriodId = payperiod[0].Payfrequencyid;
            }
            else
            {
                return Ok("No pay period found for the given company ID.");
            }

            if (file == null || file.Length == 0)
                return Ok ("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            string[] Code = companyCode.ToString().Split('(');
            DirName += Code[0].ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            DirName += "\\" + payPeriod.ToString();
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
            DataSet ds =new DataSet("NewDataSet");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return Ok ("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _ionboarding.PostNewJoineeData(xmlInput, companyCode, companyId, payPeriodId, serverpath, userId, schema);

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

        [HttpPost]
        [Route("PostAttendanceData")]
        public async Task<IActionResult> PostAttendanceData(IFormFile file, [FromForm] string companyCode,
           [FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] string payPeriod, [FromForm] string userId) 
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            string[] Code = companyCode.ToString().Split('(');
            DirName += Code[0].ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            DirName += "\\" + payPeriod.ToString();
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
            return Ok("Excel sheet is empty or not formatted correctly.");
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

            var response = await _ionboarding.PostAttendanceData(xmlInput, companyId, payPeriodId, serverpath, xmlInput2, userId, schema);
            return Ok(response);
        }

        [HttpPost]
        [Route("VerifyAttendanceHeaders")]
        public async Task<IActionResult> VerifyAttendanceHeaders(IFormFile file, [FromForm] string companyCode,
           [FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] string payPeriod)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            string[] Code = companyCode.ToString().Split('(');
            DirName += Code[0].ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            DirName += "\\" + payPeriod.ToString();
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
            DataSet ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)
                return Ok("Excel sheet is empty or not formatted correctly.");
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
            dscolumns.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();

            var response = await _ionboarding.VerifyAttendanceHeaders(xmlInput, companyId, payPeriodId, serverpath, schema);

            return Ok(response);
        }


        [HttpPost]
        [Route("PostOneTimeInputData")]
        public async Task<IActionResult> PostOneTimeInputData(IFormFile file, [FromForm] string companyCode,
           [FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] string payPeriod, [FromForm] string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            string[] Code = companyCode.ToString().Split('(');
            DirName += Code[0].ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            DirName += "\\" + payPeriod.ToString();
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
            DataSet ds = new DataSet();
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)
                return Ok("Excel sheet is empty or not formatted correctly.");
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

            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();

            var response = await _ionboarding.PostOneTimeInputData(xmlInput, companyId, payPeriodId, serverpath, userId, schema);

            return Ok(response);
        }

        [HttpGet, Route("GetAllFinalSubmitDetails/{companyId}/{payPeriodId}/{inputId}/{userId}")]
        public async Task<IActionResult> GetAllFinalSubmitDetails(int companyId, int payPeriodId, int inputId, string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            string Action = string.Empty;
            switch (inputId)
            {
                case 1:
                    Action = "GetSalary";
                    break;
                
                case 2:
                    Action = "GetOI";
                    break;
                
                //case 3:
                //    Action = "GetRevised";
                //    break;

                default:
                    Action = "GetSalary";
                    break;
            }
            var response = await _ionboarding.GetAllFinalSubmitDetails(companyId, payPeriodId, Action, userId, schema);

            return Ok(response);
        }

        [HttpPost]
        [Route("PostFinalSubmission")]
        public async Task<IActionResult> PostFinalSubmission([FromForm] string companyCode, [FromForm] int companyId, [FromForm] int payPeriodId,
            [FromForm] string payPeriod, [FromForm] string lotNumber, [FromForm] string userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (string.IsNullOrEmpty(companyCode) || string.IsNullOrEmpty(payPeriod) || string.IsNullOrEmpty(lotNumber))
            {
                return Ok("Company code, pay period, and lot number are required.");
            }
            var response = await _ionboarding.PostFinalSubmission(companyId, payPeriodId, lotNumber, userId, schema);
            return Ok(response);
        }


        [HttpPost, Route("GetPayRegister")]
        public IActionResult DownloadPayRegister([FromForm] int companyId, [FromForm] string companyCode, [FromForm] int payPeriodId, [FromForm] string payPeriod, [FromForm] int lotNumber
            , [FromForm] int inputId, [FromForm] int flag)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            try
                {
                    string Action = inputId switch
                    {
                        1 => "GetSalary",
                        2 => "GetOI",
                        _ => "GetSalary"
                    };
                    // Access config value
                    string baseDir = Path.Combine(_configuration["ClaimDocPath"].ToString());

                // Extract company code
                string companyCodeText = companyCode.Split('(')[0].Trim();

                // Build directory path
                string dirPath = Path.Combine(baseDir, companyCodeText, payPeriod, Convert.ToString(lotNumber));

                var filejson = _ionboarding.GetRegisterFilename(companyId, payPeriodId, lotNumber, Action, flag, schema);
                var fileList = JsonConvert.DeserializeObject<List<FileJson>>(filejson);
                if (fileList == null || fileList.Count == 0 || string.IsNullOrEmpty(fileList[0]?.FileName))
                {
                    return Ok(new { message = "Filename not found." });
                }
                string? fileName = fileList?[0].FileName;
                string fullPath = Path.Combine(dirPath, fileName);

                if (!System.IO.File.Exists(fullPath))
                    return Ok(new { message = "File not found." });

               var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string base64String = Convert.ToBase64String(fileBytes);
                    FileResponse fileResponse = new FileResponse();
                    fileResponse.FileName = fileName;
                    fileResponse.File = base64String;

                    return Ok(fileResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost, Route("GetNewJoineeEmployeeId")]
        public async Task<IActionResult> GetNewJoineeEmployeeId([FromForm] int companyId, [FromForm] string payPeriod, [FromForm] int lotNumber)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            DataSet ds = await _ionboarding.GetNewJoineeEmployeeId(companyId, payPeriod, lotNumber, schema);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], GetSheetName2(i));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "EmployeeId_Report" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }
                }
            }
            else
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "Failure",
                    Data = "",
                    Error = ""
                };
                return Ok(response);
            }

        }
        private string GetSheetName2(int i)
        {
            string sheetName = string.Empty;
            switch (i)
            {
                case 0:
                    sheetName = "Employee Id Creation Report";
                    break;
                case 1:
                    sheetName = "New Joiner Salary";
                    break;

                default:
                    sheetName = "";
                    return sheetName;
            }
            return sheetName;

        }

        [HttpPost, Route("GetConsolidatePayRegister")]
        public async Task<IActionResult> GetConsolidatePayRegister([FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] string lotNumber)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            DataSet ds = await _ionboarding.GetConsolidatePayRegister(companyId, payPeriodId, lotNumber, schema);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], "sheet"+(i));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "Consolidated_Payregister" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }
                }
            }
            else
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "Failure",
                    Data = "",
                    Error = ""
                };
                return Ok(response);
            }

        }

    }
}
