using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using QPay.UI.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Reports
{
    public class SGPayRegisterRepository : ISGPayRegisterRepository
    {
        private readonly DbRepository _dbRepository;

        public SGPayRegisterRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<DataSet> ReportDownload(SGPay request, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Pay_Period"] = request.Pay_Period
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_PayRegister_Consolidated_SGFormat", parameters, 1500);
        }

    }
}
