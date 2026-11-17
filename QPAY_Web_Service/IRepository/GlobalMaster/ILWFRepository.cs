using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.GlobalMaster.LWFClass;


namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface ILWFRepository
    {
        Task<DataSet> GetLWFSlabSearch(string schema);
        Task<DataSet> GetLWFSlabExporttoExcel(string schema);
        Task<LWFResponse> CreateUpdateDeleteLWFSlab(LWFSlabRequest request);
    }
}
