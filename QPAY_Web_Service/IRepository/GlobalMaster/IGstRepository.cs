using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.UI.Models.GlobalMaster;
using static QPay.UI.Models.GlobalMaster.GlobalMasters;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IGstRepository
    {
        Task<DataSet> SearchDetails(string UserId);
        Task<DataSet> ExporttoExcel(string UserId);
        Task<GstMastersResponse> Create(GstRequest createRequest);
        Task<GlobalMastersResponse> Edit(GstRequest createRequest);
        Task<GlobalMastersResponse> Delete(int GstMasterId,int UserId);
    }
}
