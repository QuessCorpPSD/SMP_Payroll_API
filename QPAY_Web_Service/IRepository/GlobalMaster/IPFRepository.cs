using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.GlobalMaster.PFClass;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IPFRepository
    {
        Task<List<PFPayCodesUI>> PFPayCodes(string schema);
        Task<List<PFCapTypeUI>> PFCapType(string schema);
        Task<DataSet> PFSearch(int CapType, int Paycode, string schema);
        Task<DataSet> GetPFDetails(int providentFundId, string schema);
        Task<DataSet> PFExporttoExcel(string CapType, string schema);
        Task<PFResponse> CreateUpdatePF(PFRequest request, string schema);
        Task<PFResponse> DeletePF(PFDeleteRequest request, string schema);
    }
}
