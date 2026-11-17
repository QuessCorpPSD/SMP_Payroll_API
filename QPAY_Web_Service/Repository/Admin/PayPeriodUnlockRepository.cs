using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.BAL.IRepository.Admin;
using QPay.DAL.Repository;
using static QPay.UI.Admin.PayPeriodUnlock;
using static QPay.UI.Models.GlobalMaster.LWFClass;

namespace QPay.BAL.Repository.Admin
{
    public class PayPeriodUnlockRepository : IPayPeriodUnlockRepository
    {
        private readonly DbRepository _dbRepository;

        public PayPeriodUnlockRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object?>
            {
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetDetailsForPayProcessAdmin", parameters, 1500);
        }

        public async Task<DataSet> Unlock(UnlockRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Allow_Reprocess_Id"] = request.Allow_Reprocess_Id,
                ["@ModifiedBy"] = request.ModifiedBy
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_UpdatePayFrequencyIsLockedStatus", parameters, 1500);
        }
    }
}
