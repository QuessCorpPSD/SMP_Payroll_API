using Dapper;
using Newtonsoft.Json;
using QPay.DAL.Repository;
using QPay.IRepository.iRepository.Common;
using QPay.UI.Common;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.IRepository.Repository.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly DbRepository _dbRepository;

        public CommonRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<CompanyPicker>> GetallCompanyCodes(int userId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@USER_ID", userId);

            var res = await this._dbRepository.GetItemsAsync(schema+".Proc_Get_AllCompanyCodebyUserId", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CompanyPicker>>(res) ?? new List<CompanyPicker>();
            }

            return new List<CompanyPicker>();
        }

        public async Task<List<PayperiodDD>> GetAllPayperiod(int companyId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_GetAllPayperiodByCompanyId", parameters);
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }
        public List<PayperiodDD> GetCurrentPayperiod(int companyId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = this._dbRepository.GetItemsAsync(schema + ".Proc_GetCurrentPayperiod", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }
        public async Task<List<MapnameDD>> GetMapNamebyCompany(int companyId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = await this._dbRepository.GetItemsAsync(schema + ".sp_GetAllMapNameByCompanyId_NewUI", parameters);
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<MapnameDD>>(res) ?? new List<MapnameDD>();
            }

            return new List<MapnameDD>();
        }
        public async Task<List<InputTypeDD>> GetAllInputType(string schema)
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_GetAllInputTypes", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InputTypeDD>>(res) ?? new List<InputTypeDD>();
            }

            return new List<InputTypeDD>();
        }
        public async Task<List<PSDStatus>> GetLotwisePSDStatus(int companyId, int payPeriodId, int lotNumber, string Payroll_Input_Type, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@pay_period_id", payPeriodId);
            parameters.Add("@lotnumber", lotNumber);
            parameters.Add("@Payroll_Input_Type", Payroll_Input_Type);


            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_InputLotStatus", parameters); //not available in db

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PSDStatus>>(res) ?? new List<PSDStatus>();
            }

            return new List<PSDStatus>();
        }

        public async Task<List<Site>> GetSitesByCompanyId(int companyId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);

            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_view_SitesByCompanyId", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Site>>(res) ?? new List<Site>();
            }

            return new List<Site>();
        }

        public async Task<List<City>> GetCityByCompanyCode(string CompanyCode, int Group_Id, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyCode", CompanyCode);
            parameters.Add("@Group_Detail_Id", Group_Id);

            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_GetCityByCompanyCode", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<City>>(res) ?? new List<City>();
            }

            return new List<City>();
        }

        public async Task<List<AllPayperiod>> GetPayPeriod(string schema)
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_GetAllPayPeriod", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<AllPayperiod>>(res) ?? new List<AllPayperiod>();
            }
            return new List<AllPayperiod>();
        }

        public async Task<List<StateUI>> GetAllState(string schema)
        {
            string storeProcedure = schema + ".sp_GetAllStates" ?? "";
            var parameter = new DynamicParameters();
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<StateUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<StateUI>>(res);
                return list?.ToList() ?? new List<StateUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<StateUI>();
            }

        }
        public async Task<List<CityUI>> GetCityByStateId(int stateId, string schema)
        {
            string storeProcedure = schema + ".sp_GetCityByStateId" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@StateId", stateId);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<CityUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<CityUI>>(res);
                return list?.ToList() ?? new List<CityUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<CityUI>();
            }

        }

        public async Task<List<Paycodes>> GetPaycodes(string schema)
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync(schema + ".sp_GetAllPaycode", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Paycodes>>(res) ?? new List<Paycodes>();
            }
            return new List<Paycodes>();
        }

        public async Task<List<Paycodes>> GetMultiCommercialPaycodes(string schema)
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync(schema + ".sp_GetAllMultiCommercialPaycode", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Paycodes>>(res) ?? new List<Paycodes>();
            }
            return new List<Paycodes>();
        }

        public async Task<List<PayperiodDD>> GetSalaryReleasePayperiod(int companyId, string Flag, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Action", Flag);
            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_Get_Frequency_ReleaseRequest_Qzone", parameters);
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }

        public async Task<List<CompanyPicker>> GetProfomaCompany(int userId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            var res = await this._dbRepository.GetItemsAsync(schema + ".GetProfomaCompany", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CompanyPicker>>(res) ?? new List<CompanyPicker>();
            }

            return new List<CompanyPicker>();
        }

    }
}
