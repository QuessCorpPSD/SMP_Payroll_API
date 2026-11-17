using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.GlobalMaster;
using QPay.UI_Domain.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.Master.Holiday;
using static QPay_Web_Domain.Models.Increment;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface ISiteMasterRepository
    {
        Task<DataSet> Search(int? companyId, int? groupId, string schema);
        Task<DataSet> GetQuessLegalEntity(string schema);

       // Task<DataSet> Create(EntityRequest request);

        Task<DataSet> ExporttoExcel(int? companyId, int? groupId, string schema);

        Task<List<PortalPayslipFormatUI>> GetPortalPayslipFormat(string schema);
        Task<SiteMasterResponse> CreateUpdateSiteMaster(CreateUpdateSitemasterRequest request, string schema);
        Task<SiteMasterResponse> UploadSiteMaster(IFormFile file, [FromForm] string User, string schema);

        //Task<DataSet> GetCriteria(int? CriteriaTypeId);
        //Task<List<CategoryUI>> GetCategory();
    }
}
