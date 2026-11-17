using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.BankInvoice;
using QPay.UI.Models.BankInvoice;
using System.Data;

namespace QPay.API.Controller.BankInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericUploadBankController : ControllerBase
    {
        private readonly IGenericUpload _repo;

        public GenericUploadBankController(IGenericUpload repo)
        {
            _repo = repo;
        }

        [HttpGet("GetUploadTypes/{createdBy}")]
        public async Task<IActionResult> GetUploadTypes(int createdBy)
        {
            var ds = await _repo.GetUploadTypes(createdBy);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] GenericUploadModel request)
        {
            var ds = await _repo.UploadFile(
                request.File,
                request.CreatedBy,
                request.UploadType
            );

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet("DownloadTemplate/{uploadType}")]
        public IActionResult DownloadTemplate(string uploadType)
        {
            DataTable dt = _repo.GetExcelColumnNames(uploadType);

            if (dt == null || dt.Columns.Count == 0 || dt.Columns.Contains("InvalidUploadType"))
            {
                return BadRequest("Invalid UploadType: " + uploadType);
            }

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "TempTemplates");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = uploadType.Replace(" ", "") + "_Template.xlsx";
            string fullPath = Path.Combine(folderPath, fileName);

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Template");

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                    ws.Cell(1, i + 1).Style.Font.Bold = true;
                }

                ws.Columns().AdjustToContents();
                workbook.SaveAs(fullPath);
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            Response.Headers["Content-Disposition"] =
                $"attachment; filename=\"{fileName}\"";

            return File(
                fileBytes,
                "application/octet-stream",
                fileName
            );
        }
    }
}