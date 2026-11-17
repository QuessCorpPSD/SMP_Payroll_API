using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.GlobalMaster.ESIClass;

namespace QPay.BAL.Repository.GlobalMaster
{

    public class PaycodeRepository : IPaycodeRepository
    {
        private readonly DbRepository _dbRepository;

        public PaycodeRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search(string strPayCode, int? intPayTypeId, int? IsTaxable, int? PayId, string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PayCode_Code"] = strPayCode,
                ["@PayTypeId"] = intPayTypeId,
                ["@IsTaxable"] = IsTaxable,
                ["@PayCode_Id"] = PayId
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema+".sp_GetAllPaycode", parameters); ;

        }

        public async Task<DataSet> GetPageType(string schema)
        {
            var parameters = new Dictionary<string, object>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllPageType", parameters);

        }
        public async Task<string> GetPayType(string schema)
        {
            List<PayTypes> response = new List<PayTypes>();
            var res = _dbRepository.QueryMultiAsync("select * from "+schema+".tbl_PayType").Result;
            return res;
        }


        public async Task<DataSet> Create(PaycodeCreateParams _params, string schema)
        {
            var payCodesResponse = new PayCodesResponse();
            payCodesResponse.PayCodesDetails = new PayCodes[1];
            payCodesResponse.PayCodesDetails[0] = _params.detail;
            string payCodesResponseSerialize = GenericSerializer<PayCodesResponse>.Serialize(payCodesResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = payCodesResponseSerialize,
                ["@mode"] = _params.mode,
                ["@CreatedBy"] = _params.userId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdatePayCode", parameters); ;

        }


    }
}
