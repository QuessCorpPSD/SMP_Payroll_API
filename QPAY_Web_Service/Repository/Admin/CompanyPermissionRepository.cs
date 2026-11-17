using Newtonsoft.Json;
using QPay.BAL.IRepository.Admin;
using QPay.DAL.Repository;
using QPay.UI.Models.Admin;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Admin
{
    public class CompanyPermissionRepository : ICompanyPermissionRepository
    {
        private readonly DbRepository _dbRepository;

        public CompanyPermissionRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<DataSet> GetUsername(string schema)
        {
            var parameters = new Dictionary<string, object>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_GetAllUserName", parameters);
        }


        public async Task<DataSet> Search(int? UserId, int? BusinessUnitId, int? CompanyPermissionId, string schema)
        {
            var parameters = new Dictionary<string, object?>    {
        { "@Userid", UserId },
        { "@Businessunitnameid", BusinessUnitId },
        { "@CompanyPermissionId", CompanyPermissionId }
        };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_GetAllCompanyPermission", parameters, 1500);
        }

        public async Task<DataSet> ExportToExcel(int? UserId, int? Businessunitnameid, string schema)
        {
            var parameters = new Dictionary<string, object?>    {
        { "@Userid", UserId },
        { "@Businessunitnameid", Businessunitnameid }

        };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_GetAllCompanyPermission_Export2Excel", parameters, 1500);
        }

        public async Task<DataSet> LoadCompany(int? BusinessUnitNameId, int? BusinessZonenName, string schema)
        {
            var parameters = new Dictionary<string, object>
            {

                ["@BusinessUnitNameId"] = BusinessUnitNameId,
                ["@BusinessZonenName"] = BusinessZonenName



            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_GetCompanyDetailsForPermission", parameters);
        }


        //    public async Task<DataSet> CreateCompanyPermission(CompanyPermission request)
        //    {
        //        string jsonData = JsonConvert.SerializeObject(request.CompanyPermissionDetails);

        //        var parameters = new Dictionary<string, object?>
        //{
        //    { "@xmlInput", jsonData },
        //    { "@mode", request.vmode },
        //    { "@CreatedBy", request.userId }
        //};

        //        return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_CreateUpdateCompanyPermission", parameters, 1500);
        //    }

        public async Task<DataSet> CreateUpdateDelete(string xml, int createdBy, string mode, string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = xml,
                ["@mode"] = mode,
                ["@CreatedBy"] = createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_CreateUpdateCompanyPermission", parameters);
        }

        public async Task<DataSet> Editdetails(int? Userid, int? Businessunitnameid, int? CompanyPermissionId, string schema)
        {
            var parameters = new Dictionary<string, object>
            {

                ["@Userid"] = Userid,
                ["@Businessunitnameid"] = Businessunitnameid,
                ["@CompanyPermissionId"] = CompanyPermissionId



            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Sp_GetAllCompanyPermissionDetails", parameters);
        }




    }
}
