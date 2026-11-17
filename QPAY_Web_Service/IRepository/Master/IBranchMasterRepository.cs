using QPay.UI.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Master
{
   public  interface IBranchMasterRepository
    {
        Task<DataSet> Search(int? StateId, string? BranchName);
        Task<DataSet> GetAllPtState();
        Task<DataSet> Create(BranchMasterXml items);
        Task<DataSet> ExporttoExcel(int? StateId, string? BranchName);
    }
}
