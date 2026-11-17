using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using QPay.UI_Domain.Models.Master;
using System.Data;


namespace QPay.IRepository.Repository.Master
{
    public class LeaveMasterRepository: ILeaveMasterRepository
    {
        private readonly DbRepository _dbRepository;

        public LeaveMasterRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<LeaveMaster>> GetAllLeaveMaster(int? companyId, string? siteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            parameters.Add("@Site_id", siteId);

            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllLeaveType", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<LeaveMaster>>(res) ?? new List<LeaveMaster>();
            }

            return new List<LeaveMaster>();
        }
        public async Task<List<LeavetypeDD>> GetLeaveTypeDD()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("Proc_GetLeaveType_New", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<LeavetypeDD>>(res) ?? new List<LeavetypeDD>();
            }

            return new List<LeavetypeDD>();
        }
        public async Task<string> PostAddLeaveMaster(LeaveMasterAdd leavemasteradd)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", leavemasteradd.CompanyId);
            parameters.Add("@SiteId", leavemasteradd.SiteId);
            parameters.Add("@Sitename", leavemasteradd.SiteName);
            parameters.Add("@LeaveType", leavemasteradd.Leavetype);
            parameters.Add("@CreatedBy", leavemasteradd.UserId);


            var res = await this._dbRepository.GetItemsAsync("Proc_AddLeaveMaster", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public DataSet LeaveMasterExport(int companyId, int siteCode)
        {
            DataSet ds = this._dbRepository.LeaveMasterExport(companyId, siteCode);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and site.");
            }
        }
        public async Task<string> PostDeleteLeave(int leavemasterid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@leavemasterid", leavemasterid);
            var res = await this._dbRepository.GetItemsAsync("Proc_Delete_LeaveMaster", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
    }
}
