using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static QPay.UI_Domain.Models.Master.VendorEmployee;

namespace QPay.BAL.IRepository.Master
{
    public interface IVendorEmployeeRepository
    {
        Task<DataSet> GetVendorEmployee(string CompanyId, string SiteId, string EmployeeCode,
            string EActive);
        Task<DataSet> GetVendorEmployeeTemplate();
        Task<VendorEmployeeResponse> ImportVendorEmployee(IFormFile file, [FromForm] string User);
    }
}
