using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Models.GlobalMaster;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.GlobalMaster.ESIClass;

namespace QPay.BAL.Repository.GlobalMaster
{
    public class CityRepository: ICityRepository
    {
        private readonly DbRepository _dbRepository;

        public CityRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<List<City>> GetAllCity(string? cityName, int? stateId, int? cityId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CityName", cityName);
            parameters.Add("StateId", stateId);
            parameters.Add("CityID", cityId);


            var res = await _dbRepository.GetItemsAsync(schema + ".sp_GetAllCity", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<City>>(res) ?? new List<City>();
            }

            return new List<City>();
        }

        public async Task<string> Create(string xml, string mode, int createdBy, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xml);
            parameters.Add("@mode", mode);
            parameters.Add("@CreatedBy", createdBy);

            var res = await this._dbRepository.GetItemsAsync(schema + ".sp_CreateUpdateCity2", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<List<Circle>> GetAllCircle(int stateId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("State_id", stateId);

            var res = await _dbRepository.GetItemsAsync(schema + ".Sp_GetCircelStateby", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Circle>>(res) ?? new List<Circle>();
            }

            return new List<Circle>();
        }
    }
}
