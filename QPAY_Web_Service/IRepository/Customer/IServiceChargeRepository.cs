using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Customer;
using QPay.UI.Models;
using QPay.UI.SalaryAdvance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Customer.Company;

namespace QPay.BAL.IRepository.Customer
{
    public interface IServiceChargeRepository
    {
        //Task<DataTable> Search(string action, int? companyId, string xml);
        //Task<DataSet> ExportToExcel(string action, int? companyId, string xml);
        Task<DataSet> serviceChargeMaster();
        Task<DataSet> serviceChargeMasterNew(int companyId);
        Task<DataSet> GetUnitType();
        Task<DataSet> GetUnitTypeBilltoRate();
        Task<DataSet> serviceChargeType(int? serviceChargeId);
        Task<ServiceChargeResponse> Create(ServiceChargeRequest request);
        Task<ServiceChargeResponse> ServiceChargeDelete(ServiceChargeDeleteRequest request);

        Task<ServiceChargeResponse> FileUpload(IFormFile file, [FromForm] int ServiceChargeMaster, [FromForm] int ServiceChargeType,
              [FromForm] int SlabType, [FromForm] int SlabInnerType, [FromForm] int CreatedBy);
        Task<DataSet> GetAllServiceCharge(int companyId, int Service_Charge_Master_Id, int Service_Charge_Type_Id);
        Task<ServiceChargeResponse> FileUploadServiceFeeBilltoRateFileUpload(IFormFile file, [FromForm] int CreatedBy);
        
    }
}
