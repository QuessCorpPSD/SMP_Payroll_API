using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.AMSCommon;
using QPay.DAL.Repository;
using QPay.IRepository.iRepository.Common;
using QPay.UI.Models;
using QPay.UI.Models.AMSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.AMSCommon
{
    public class AMSCommonRepository : IAMSCommonRepository
    {
        private readonly DbRepository _dbRepository;

        public AMSCommonRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<CommonDropdown>> GetAllEntity()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllEntity", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CommonDropdown>>(res) ?? new List<CommonDropdown>();
            }

            return new List<CommonDropdown>();
        }

        public async Task<List<CommonDropdown>> GetAllEmploymentType()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("AMS_GetAllEmploymentType", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CommonDropdown>>(res) ?? new List<CommonDropdown>();
            }

            return new List<CommonDropdown>();
        }

        public async Task<List<CommonDropdown>> GetAllState()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("AMS_GetAllState", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CommonDropdown>>(res) ?? new List<CommonDropdown>();
            }

            return new List<CommonDropdown>();
        }
        public async Task<List<CommonDropdown>> GetAllCompanyByEntity(int entityId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Entity_Id", entityId);

            var res = await this._dbRepository.GetItemsAsync("AMS_GetAllCompanybyEntity", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CommonDropdown>>(res) ?? new List<CommonDropdown>();
            }

            return new List<CommonDropdown>();
        }

        public async Task<List<CommonDropdown>> GetAllDesignationbyCompany(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);

            var res = await this._dbRepository.GetItemsAsync("AMS_GetAllDesignationbyCompany", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CommonDropdown>>(res) ?? new List<CommonDropdown>();
            }

            return new List<CommonDropdown>();
        }

        public async Task<List<CommonDropdown>> GetAllBandbyCompany(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);

            var res = await this._dbRepository.GetItemsAsync("AMS_GetAllBandbyCompany", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CommonDropdown>>(res) ?? new List<CommonDropdown>();
            }

            return new List<CommonDropdown>();
        }

        public async Task<List<CommonDropdown>> GetAllCitybyState(int stateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StateId", stateId);

            var res = await this._dbRepository.GetItemsAsync("AMS_GetAllCitybyState", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CommonDropdown>>(res) ?? new List<CommonDropdown>();
            }

            return new List<CommonDropdown>();
        }
    }
}
