using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Customer.PermHire;

namespace QPay.BAL.IRepository.Customer
{
    public interface IPermHireRepository
    {
        Task<DataSet> GetServiceChargeType(ServiceChargeTypeRequest request);
        Task<PermHireResponse> ServiceChargeTypeCreate([FromBody] ServiceChargeTypeDto request);
        Task<DataSet> GetJobCategorySearch(JobCategoryRequest request);
        Task<PermHireResponse> JobCategoryCreate([FromBody] JobCategoryDto request);
        Task<DataSet> GetJobCategory();
        Task<DataSet> GetJobSubCategorySearch(int CompanyId);
        Task<DataSet> GetJobSubCategoryExport(int CompanyId);
        Task<PermHireResponse> JobSubCategoryCreate([FromBody] JobSubCategoryDto request);
        Task<DataSet> GetPermHireMasterSearch(PermHireMasterRequest request);
        Task<PermHireResponse> PermHireMasterApproveReject([FromBody] PermHireMasterDto request);
    }

}
