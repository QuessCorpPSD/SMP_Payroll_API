using QPay.UI.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Master
{
   public  interface ITDSSlabMasterRepository
    {
        Task<DataSet> Search(string SearchCategory, int? tdsSlabId, string schema);
        Task<DataSet> GetFinancialYear();
        Task<DataSet> GetAllTDSSlabDetail(int? tdsSlabId);
        Task<DataSet> Create(TDSSlabRequest request, string schema);
        Task<DataSet> ExporttoExcel(int? FinancialYearId, string SearchCategory);

    }
}
