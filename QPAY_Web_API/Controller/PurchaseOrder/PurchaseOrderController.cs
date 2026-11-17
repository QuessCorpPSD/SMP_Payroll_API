using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.PurchaseOrder;
using QPay.UI_Domain.Models.PurchaseOrder;
using System.Data;
using System.Xml.Serialization;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;

namespace QPay_Web_API.Controller.Master.PurchaseOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IConfiguration _configuration;
        public PurchaseOrderController(IConfiguration configuration, IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _configuration = configuration;
        }
        [HttpGet, Route("GetCategory")]
        public async Task<IActionResult> GetCategory()
        {
            var catgory = await _purchaseOrderRepository.GetPOCategory();
            return Ok(catgory);
        }
        [HttpGet, Route("GetInvoiceType")]
        public async Task<IActionResult> GetInvoiceType()
        {
            var invoiceType = await _purchaseOrderRepository.GetInvoiceType();
            return Ok(invoiceType);
        }
        [HttpGet, Route("GetBillingType/{billingType}")]
        public async Task<IActionResult> GetBillingType(string billingType)
        {
            var invoiceType = await _purchaseOrderRepository.GetBillAndPricingType(billingType);
            return Ok(invoiceType);
        }
        [HttpPost, Route("POQuantityValue")]
        public async Task<IActionResult> POQuantityValue(PO_QuantityModelRequest pO_QuantityModelRequest)
        {
            var invoiceType = await _purchaseOrderRepository.GetPOQuantityByCompanyId(pO_QuantityModelRequest.StartDate ?? "", pO_QuantityModelRequest.EndDate ?? "", Convert.ToInt32(pO_QuantityModelRequest.QUANTITYTYPE), pO_QuantityModelRequest.CompanyId);
            return Ok(invoiceType);
        }
        [HttpPost, Route("POBulkUpload")]
        public async Task<IActionResult> POBulkUpload(POBulkModelRequest bulkModelRequest)
        {
            List<BulkUploadResponse> bulkUploadResponses = new List<BulkUploadResponse>();
            BulkUploadResponse uploadResponse = new BulkUploadResponse();
            var filePath = _configuration["POFilePath"] ?? "";
            var fileName = Path.GetFileNameWithoutExtension(bulkModelRequest.File[0].name) + DateTime.Now.ToString("DDMMYYYYhhmmss") + ".xlsx"; //+ bulkModelRequest.File[0].type;
            var fileBytes = Convert.FromBase64String(bulkModelRequest.File[0].content);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var filePathName = Path.Combine(filePath, fileName);
            await System.IO.File.WriteAllBytesAsync(filePathName, fileBytes);
            var PO_data = ReadExcelToDataTable(filePathName);
            if (PO_data != null)
            {
                if (PO_data.Rows.Count > 0)
                {
                    if (PO_data?.Columns["Company_Code"]?.ToString() == "Company_Code" && PO_data?.Columns["PoNumber"]?.ToString() == "PoNumber" && PO_data?.Columns["POValue"]?.ToString() == "POValue")
                    {
                        var doj = from dr in PO_data.AsEnumerable()
                                  select new DOJ
                                  {
                                      CompanyCode = dr.Field<string>("Company_Code") ?? "",
                                      PoNumber = dr.Field<string>("PoNumber") ?? "",
                                      POValue = dr.Field<string>("POValue") ?? "",
                                      PODate = dr.Field<string>("PODate") ?? "",
                                      StartDate = dr.Field<string>("PODate") ?? "",
                                      EndDate = dr.Field<string>("EndDate") ?? "",
                                      PricingType = dr.Field<string>("PricingType") ?? "",
                                      CurrencyType = dr.Field<string>("CurrencyType") ?? "",
                                      POQuantityType = dr.Field<string>("POQuantityType") ?? "",
                                      BillingType = dr.Field<string>("BillingType") ?? "",
                                      InternalExternal = dr.Field<string>("InternalExternal") ?? "",
                                      ExtentionEndDate = dr.Field<string>("ExtentionEndDate") ?? "",
                                      ExtentionPOValue = dr.Field<string>("ExtentionPOValue") ?? "",
                                      POCategory = dr.Field<string>("POCategory") ?? "",
                                      Mode = dr.Field<string>("Mode") ?? ""

                                  };
                        var poxml = ConvertToXml(doj.ToList());
                        if (!string.IsNullOrEmpty(poxml))
                        {
                            var status = _purchaseOrderRepository.BulkPoUpload("Upload", poxml, bulkModelRequest.EmployeeId.ToString());
                            return Ok(status);
                        }
                        else
                        {
                            uploadResponse.Error_Message = "Unable to convert xml";
                            bulkUploadResponses.Add(uploadResponse);
                        }


                    }
                    else
                    {
                        uploadResponse.Error_Message = "Column headers are not matching";
                        bulkUploadResponses.Add(uploadResponse);
                    }

                }
                else
                {
                    uploadResponse.Error_Message = "Record Not Found";
                    bulkUploadResponses.Add(uploadResponse);
                }


            }
            else
            {
                uploadResponse.Error_Message = "Unable to extract the excel";
                bulkUploadResponses.Add(uploadResponse);
            }

            return Ok(bulkUploadResponses);


        }
        [HttpGet, Route("PoSearchByCompanyId/{companyId}")]
        public async Task<IActionResult> PoSearchByCompanyId(int companyId)
        {
            var res = await _purchaseOrderRepository.POSearch(companyId);
            return Ok(res);

        }
        [HttpPost, Route("POSearchCompanyAndPoNumber")]
        public async Task<IActionResult> POSearchCompanyAndPoNumber(POSearchRequestModel searchRequestModel)
        {
            var search = await _purchaseOrderRepository.GetPOsAndCustomersAsync(searchRequestModel.companyID, searchRequestModel.pricingType, searchRequestModel.PONumber, searchRequestModel.flag);
            return Ok(search);
        }
        [HttpPost, Route("EmployeePOSearch")]
        public IActionResult EmployeePOSearch(EmployeePOSearchModel employeePOSearchModel)
        {
            var search = _purchaseOrderRepository.EmployeePOSearch(employeePOSearchModel.companyId, employeePOSearchModel.poNumber, employeePOSearchModel.employeeId, employeePOSearchModel.status, employeePOSearchModel.pricingType);
            //return Ok(search);

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("MainPoSearch/{companyId}/{PricingType}/{PONumber}")]
        public IActionResult MainPoSearch(int companyId, string PricingType, string PONumber)
        {
            var search = _purchaseOrderRepository.MainPoSearch(companyId, PricingType,
                PONumber.Replace('_', '/'));

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpPost("CreateMainPo")]
        public async Task<IActionResult> CreateMainPo(CreateMainPoRequest request)
        {
            var res = await _purchaseOrderRepository.CreateMainPo(request);
            return Ok(res);
        }

        [HttpPost]
        [Route("FileUploadMainPo")]
        public async Task<IActionResult> FileUploadMainPo(IFormFile file, [FromForm] string PoNumber,
          [FromForm] string File_Name, [FromForm] string File_Path, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _purchaseOrderRepository.FileUploadMainPo(file, PoNumber, File_Name, File_Path, CreatedBy);
            return Ok(result);
        }

        [HttpGet, Route("GetPoCreateTemplate/{flag}/{CreatedBy}")]
        public IActionResult GetPoCreateTemplate(string flag, string CreatedBy)
        {
            var ds = _purchaseOrderRepository.GetPoCreateTemplate(flag, CreatedBy);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("BulkPOCreate")]
        public async Task<IActionResult> BulkPOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _purchaseOrderRepository.BulkPOCreate(file, flag, CreatedBy);
            return Ok(result);
        }

        [HttpGet, Route("GetPOStatus")]
        public async Task<IActionResult> GetPOStatus()
        {
            var catgory = await _purchaseOrderRepository.GetPOStatus();
            return Ok(catgory);
        }

        [HttpPost, Route("EmployeePoView")]
        public IActionResult EmployeePoView(EmployeePoView employeePoView)
        {
            var search = _purchaseOrderRepository.EmployeePoView(employeePoView.companyId,
                employeePoView.poNumber,
                employeePoView.clientEmployeeId,
                employeePoView.status,
                employeePoView.pricingType);

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("EmployeePoViewApprove")]
        public IActionResult EmployeePoViewApprove(EmployeePoViewApprove employeePoViewApprove)
        {
            var search = _purchaseOrderRepository.EmployeePoViewApprove(employeePoViewApprove.companyId,
                employeePoViewApprove.poNumber,
                employeePoViewApprove.pricingType,
                employeePoViewApprove.employeeId,
                employeePoViewApprove.status,
                employeePoViewApprove.employeeListId);

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("EmployeePoViewEmpdtls")]
        public IActionResult EmployeePoViewEmpdtls(EmployeePoViewEmpDtls employeePoViewempdtls)
        {
            var search = _purchaseOrderRepository.EmployeePoViewEmpdtls(employeePoViewempdtls.companyId,
                employeePoViewempdtls.poNumber,
                employeePoViewempdtls.pricingType,
                employeePoViewempdtls.employeeId);

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("EmployeePoViewApproveNew")]
        public IActionResult EmployeePoViewApproveNew(EmployeePoViewApproveNew employeePoViewApprovenew)
        {
            var search = _purchaseOrderRepository.EmployeePoViewApproveNew(employeePoViewApprovenew.companyId,
                employeePoViewApprovenew.poNumber,
                employeePoViewApprovenew.pricingType,
                employeePoViewApprovenew.employeeId,
                employeePoViewApprovenew.status);

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpPost("EmployeePoNewCreation")]
        public async Task<IActionResult> EmployeePoNewCreation(EmployeePoNewCreationModel employeePoNewCreation)
        {
            var res = await _purchaseOrderRepository.EmployeePoNewCreation(employeePoNewCreation);
            return Ok(res);
        }

        [HttpPost("EmployeePoEdit")]
        public async Task<IActionResult> EmployeePoEdit(EmployeePoEditModel employeePoEdit)
        {
            var res = await _purchaseOrderRepository.EmployeePoEdit(employeePoEdit);
            return Ok(res);
        }

        [HttpPost("EmployeePoInsertRevised")]
        public async Task<IActionResult> EmployeePoInsertRevised(EmployeePoInsertModel employeePoInsertModel)
        {
            var res = await _purchaseOrderRepository.EmployeePoInsertRevised(employeePoInsertModel);
            return Ok(res);
        }

        [HttpGet, Route("EmployeePoCreateTemplate/{flag}/{CreatedBy}")]
        public IActionResult EmployeePoCreateTemplate(string flag, string CreatedBy)
        {
            var ds = _purchaseOrderRepository.EmployeePoCreateTemplate(flag, CreatedBy);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("BulkEmployeePOCreate")]
        public async Task<IActionResult> BulkEmployeePOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _purchaseOrderRepository.BulkEmployeePOCreate(file, flag, CreatedBy);
            return Ok(result);
        }

        [HttpGet, Route("GetPoItemType")]
        public async Task<IActionResult> GetPoItemType()
        {
            var invoiceType = await _purchaseOrderRepository.GetPoItemType();
            return Ok(invoiceType);
        }

        [HttpPost, Route("GetPOValue")]
        public async Task<IActionResult> GetPOValue(POValue_Request poValue_Request)
        {
            var invoiceType = await _purchaseOrderRepository.GetPOValue(poValue_Request.company_Id ?? "", poValue_Request.site_Id ?? "",
                poValue_Request.quantity_type, poValue_Request.quantity, poValue_Request.po_Rate);
            return Ok(invoiceType);
        }

        [HttpPost]
        [Route("FileUploadEmployeePo")]
        public async Task<IActionResult> FileUploadEmployeePo(IFormFile file, [FromForm] string PoNumber,
          [FromForm] string File_Name, [FromForm] string EmployeeId, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _purchaseOrderRepository.FileUploadEmployeePo(file, PoNumber, File_Name, EmployeeId, CreatedBy);
            return Ok(result);
        }

        [HttpPost("EmployeePoDelete")]
        public async Task<IActionResult> EmployeePoDelete(EmployeePoDeleteModel employeePoDeleteModel)
        {
            var res = await _purchaseOrderRepository.EmployeePoDelete(employeePoDeleteModel.EmployeeListID,
               employeePoDeleteModel.CreatedBy);
            return Ok(res);
        }

        public static string ConvertToXml(List<DOJ> dojList)
        {
            var serializer = new XmlSerializer(typeof(List<DOJ>), new XmlRootAttribute("DocumentElement"));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, dojList);
                return stringWriter.ToString();
            }
        }
        public static DataTable ReadExcelToDataTable(string filePath, bool hasHeader = true)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // First sheet
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
}
