using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using QPay.UI.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Master
{
    public class BranchMasterRepository : IBranchMasterRepository
    {
        private readonly DbRepository _dbRepository;

        public BranchMasterRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<DataSet> Search(int? StateId, string? BranchName)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@pt_state_id"] = StateId,
                ["@branch_name"] = BranchName,
                ["@mode"] = "Search",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spBranch_Search", parameters); ;

        }

        public async Task<DataSet> GetAllPtState()
        {
            var parameters = new Dictionary<string, object>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetPTState", parameters); ;

        }


        public async Task<DataSet> Create(BranchMasterXml items)
        {
            var BranchMasterResponse = new BranchMasterResponse();
            BranchMasterResponse.BranchMasterDetails = new BranchMaster[1];
            BranchMasterResponse.BranchMasterDetails[0] = items.details;

            string BranchMasterResponseSerialize = GenericSerializer<BranchMasterResponse>.Serialize(BranchMasterResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = BranchMasterResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.userId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spBranch_CreateUpdate", parameters); ;

        }
        public async Task<DataSet> ExporttoExcel(int? StateId, string? BranchName)
        {
            var parameters = new Dictionary<string, object>
            {

                ["@pt_state_id"] = StateId,
                ["@branch_name"] = BranchName,
                ["@mode"] = "exporttoexcel",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spBranch_Search", parameters); ;
        }


    }
}
