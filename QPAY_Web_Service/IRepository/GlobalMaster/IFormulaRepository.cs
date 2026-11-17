using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IFormulaRepository
    {
        Task<DataSet> Search(int? payCodeId, string schema);
        //Task<DataSet> GetFormulaPayCodeList();
        Task<DataSet> GetPayCategory(int companyId, string schema);
        Task<DataSet> Create(FormulasRequest request, string schema);
        Task<DataSet> CreateMC(MCFormulasRequest request, string schema);
        Task<DataSet> GetPayrollType(string schema);
        Task<DataSet> MCSearch(string schema);

        //Task<DataSet> GetCriteria(int? CriteriaTypeId);
        //Task<List<CategoryUI>> GetCategory();
    }
}
