using QPay.UI.Models.AMSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.AMSCommon
{
    public interface IAMSCommonRepository
    {
        Task<List<CommonDropdown>> GetAllEntity();
        Task<List<CommonDropdown>> GetAllEmploymentType();
        Task<List<CommonDropdown>> GetAllState();
        Task<List<CommonDropdown>> GetAllCompanyByEntity(int entityId);
        Task<List<CommonDropdown>> GetAllDesignationbyCompany(int companyId);
        Task<List<CommonDropdown>> GetAllBandbyCompany(int companyId);
        Task<List<CommonDropdown>> GetAllCitybyState(int stateId);




    }
}
