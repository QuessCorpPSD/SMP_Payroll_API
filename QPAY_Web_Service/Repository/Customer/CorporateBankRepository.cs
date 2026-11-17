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
    public class CorporateBankRepository : ICorporateBankRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public CorporateBankRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Bank_Name"] = "",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_SerarchCorporateBank_API", parameters, 1500);
        }

        public async Task<DataSet> Create(CorporateBankRequest items, string schema)
        {
            var parentdata = GenericSerializer<CorporateBank>.Serialize(items.parentDetail);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateCorporateBank", parameters);
        }

    }
}
