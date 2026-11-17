using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI_Domain.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;


namespace QPay.BAL.IRepository.PurchaseOrder
{
    public interface IPurchaseOrderRepository
    {
        Task<List<POCategoryUI>> GetPOCategory();
        Task<List<InvoiceTypeUI>> GetInvoiceType();
        Task<List<RAD_SP_INV_SHOWLIST_BYGROUPUI>> GetBillAndPricingType(string groupName);
        Task<POQuantityUI> GetPOQuantityByCompanyId(string startDate, string endDate, int poQuantityType, int companyId);
        Task<List<BulkUploadResponse>> BulkPoUpload(string actionType, string xmlData, string employeeCode);
        Task<POSummaryUI> GetPOsAndCustomersAsync(int company_id, string pricingType, string PONumber, string flag);
        Task<List<POUI>> POSearch(int companyId);
        DataSet EmployeePOSearch(int companyId, string poNumber, string employeeId, string status, string pricingType);
        DataSet MainPoSearch(int companyId, string PricingType, string PONumber);
        Task<PoResponse> CreateMainPo(CreateMainPoRequest request);
        Task<PoResponse> FileUploadMainPo(IFormFile file, [FromForm] string PoNumber,
          [FromForm] string File_Name, [FromForm] string File_Path, [FromForm] string CreatedBy);
        DataSet GetPoCreateTemplate(string flag, string CreatedBy);
        Task<PoResponse> BulkPOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy);
        Task<List<POStatus>> GetPOStatus();

        DataSet EmployeePoView(int companyId, string poNumber, string clientEmployeeID, string status,
            string pricingType);
        DataSet EmployeePoViewApprove(int companyId, string poNumber, string pricingType, string employeeId,
            string status, string employeeListId);
        DataSet EmployeePoViewEmpdtls(int companyId, string poNumber, string pricingType, string employeeId);
        DataSet EmployeePoViewApproveNew(int companyId, string poNumber, string pricingType, string employeeId,
            string status);
        Task<PoResponse> EmployeePoNewCreation(EmployeePoNewCreationModel employeePoNewCreation);
        Task<PoResponse> EmployeePoEdit(EmployeePoEditModel employeePoEdit);
        Task<PoResponse> EmployeePoInsertRevised(EmployeePoInsertModel employeePoInsertModel);
        DataSet EmployeePoCreateTemplate(string flag, string CreatedBy);
        Task<PoResponse> BulkEmployeePOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy);
        Task<List<ItemTypeUI>> GetPoItemType();
        Task<POValueUI> GetPOValue(string company_Id, string site_Id, string quantity_type, string quantity, string po_Rate);
        Task<PoResponse> FileUploadEmployeePo(IFormFile file, [FromForm] string PoNumber,
 [FromForm] string File_Name, [FromForm] string EmployeeId, [FromForm] string CreatedBy);
        Task<PoResponse> EmployeePoDelete(string EmployeeListID, string CreatedBy);
    }
}
