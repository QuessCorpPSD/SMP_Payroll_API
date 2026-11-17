using QPay.UI.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Master
{
    public interface IComputationRuleRepository
    {
        Task<DataSet> Search(int? FinancialYearId, string Category, string TaxId);
        Task<DataSet> GetFinancialYear();
        Task<DataSet> Create(ComputationRuleXml items);
        Task<DataSet> ExporttoExcel(int? FinancialYearId, string Category, string TaxId);
    }
}
