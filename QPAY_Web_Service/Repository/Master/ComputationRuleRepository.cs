using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
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
    public class ComputationRuleRepository : IComputationRuleRepository
    {
        private readonly DbRepository _dbRepository;

        public ComputationRuleRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<DataSet> GetFinancialYear()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@FinancialYearId"] = "",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetFinancialYear", parameters); ;

        }
        public async Task<DataSet> Search(int? FinancialYearId, string Category, string TaxId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@FinancialYear_Id"] = FinancialYearId,
                ["@Category"] = Category,
                ["@Tax_Id"] = TaxId,
                ["@Computation_Rule_Id"] = "",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllComputationRule", parameters); ;

        }
        public async Task<DataSet> Create(ComputationRuleXml items)
        {

            var ComputationRuleResponse = new ComputationRuleResponse();
            ComputationRuleResponse.ComputationRuleDetails = new ComputationRule[1];
            ComputationRuleResponse.ComputationRuleDetails[0] = items.details;

            string formulaResponseSerialize = GenericSerializer<ComputationRuleResponse>.Serialize(ComputationRuleResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = formulaResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.userId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateComputationRule", parameters); ;

        }

        public async Task<DataSet> ExporttoExcel(int? FinancialYearId, string Category, string TaxId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@FinancialYear_Id"] = FinancialYearId,
                ["@Category"] = Category,
                ["@Tax_Id"] = TaxId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllComputationRule_ExportToExcel", parameters); ;

        }
    }
}
