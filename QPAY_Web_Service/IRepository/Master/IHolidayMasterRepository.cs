using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI_Domain.Models.Master.Holiday;

namespace QPay.BAL.IRepository.Master
{
    public interface IHolidayMasterRepository
    {
        Task<DataSet> GetHolidayCompanywise(int CompanyId, int SiteId);
        Task<DataSet> GetHolidayTemplate();
        Task<HolidayMasterResponse> UploadHolidayMaster(IFormFile file, [FromForm] string User);
        Task<HolidayMasterResponse> SaveUpdateDeleteHolidayMaster([FromBody] HolidayMasterRequest request);
    }
}
