using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Master;
using QPay.UI.Models;
using QPay.UI_Domain.Models.Master;
using System.Data;

namespace QPay_Web_API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceRuleController : ControllerBase
    {
        private readonly IInvoiceRuleRepository _invoiceRule;
        private readonly IConfiguration _configuration;
        public InvoiceRuleController(
                    IInvoiceRuleRepository invoiceRule, IConfiguration configuration)
        {
            this._invoiceRule = invoiceRule;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllInvoiceRule/{companyId}/{siteId}")]
        public async Task<IActionResult> GetAllInvoiceRule(int? companyId, string? siteId)
        {
            var response = await _invoiceRule.GetAllInvoiceRule(companyId, siteId);

            return Ok(response);
        }
        [HttpPost("PostAddInvoiceRule")]
        public async Task<IActionResult> PostAddInvoiceRule([FromBody] InvoiceRuleAdd invoiceruleAdd)
        {
            if (!ModelState.IsValid)
                return Ok(ModelState);
            var response = await _invoiceRule.PostAddInvoiceRule(invoiceruleAdd);
            return Ok(response);
        }

        [HttpPost("PostEditInvoiceRule")]
        public async Task<IActionResult> PostEditInvoiceRule([FromBody] InvoiceRuleEdit invoiceruleEdit)
        {
            if (!ModelState.IsValid)
                return Ok(ModelState);
            var response = await _invoiceRule.PostEditInvoiceRule(invoiceruleEdit);
            return Ok(response);
        }

        [HttpPost("PostDeleteInvoiceRule")]
        public async Task<IActionResult> PostDeleteInvoiceRule([FromBody] int invoicingRulesID)
        {

                var response = await _invoiceRule.PostDeleteInvoiceRule(invoicingRulesID);
                return Ok(response);

        }
        [HttpPost]
        [Route("GetInvoiceRuleTemplate")]
        public IActionResult GetInvoiceRuleTemplate([FromForm] int companyId,[FromForm] string companyCode, [FromForm] string siteName)
        {

            DataSet ds = _invoiceRule.GetInvoiceRuleTemplate(companyId, siteName);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("InvoiceRule");
                ws.Cell(1, 1).InsertTable(ds.Tables[0], "NewDataSet", true);

                var table = ws.Table("NewDataSet");
                table.ShowAutoFilter = false;
                table.Theme = XLTableTheme.None;

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());

                    FileResponse fileResponse = new FileResponse();
                    string fileName = companyCode;
                    fileResponse.FileName = "InvoiceRule_Template_" + fileName + ".xlsx";
                    fileResponse.File = bytes;
                    return Ok(fileResponse);
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
        [HttpPost]
        [Route("PostInvoiceRuleUpload")]
        public async Task<IActionResult> PostInvoiceRuleUpload(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "InvoiceRule";
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
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return Ok("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _invoiceRule.PostInvoiceRuleUpload(xmlInput, userId);

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
        [Route("InvoiceRuleExport")]
        public IActionResult InvoiceRuleExport([FromForm] int companyId, [FromForm] int siteCode)
        {

            DataSet ds = _invoiceRule.InvoiceRuleExport(companyId, siteCode);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("InvoiceRule");
                ws.Cell(1, 1).InsertTable(ds.Tables[0], "NewDataSet", true);

                var table = ws.Table("NewDataSet");
                table.ShowAutoFilter = false;
                table.Theme = XLTableTheme.None;

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());

                    FileResponse fileResponse = new FileResponse();
                    string fileName = DateTime.Now.ToString();
                    fileResponse.FileName = "InvoiceRule_Export_" + fileName + ".xlsx";
                    fileResponse.File = bytes;
                    return Ok(fileResponse);
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
