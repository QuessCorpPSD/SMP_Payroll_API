using ClosedXML.Excel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository
{
    public class ITCalenderRepository : IITCalenderRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ITCalenderRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? companyId, int? financialYearId, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = companyId,
                ["@FinancialYearId"] = financialYearId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllITCalendarDetails", parameters, 1500);
        }


        public async Task<DataSet> GetFinancialYear(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@FinancialYearId"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetFinancialYear", parameters, 1500);
        }


        public async Task<DataSet> Create(ITCalenderRequest items, string schema)
        {
            var itCalenderResponse = new ITCalenderResponse();
            itCalenderResponse.ITCalender = new ITCalenderDetails[1];
            itCalenderResponse.ITCalender[0] = items.parentDetail;
            string itCalenderResponseSerialize = GenericSerializer<ITCalenderResponse>.Serialize(itCalenderResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = itCalenderResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateITCalender1", parameters);
        }

    }
}
