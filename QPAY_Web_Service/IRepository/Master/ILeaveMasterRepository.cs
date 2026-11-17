using QPay.UI_Domain.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Master
{
    public interface ILeaveMasterRepository
    {
        Task<List<LeaveMaster>> GetAllLeaveMaster(int? companyId, string? siteId);
        Task<List<LeavetypeDD>> GetLeaveTypeDD();
        Task<string> PostAddLeaveMaster(LeaveMasterAdd leavemasteradd);
        DataSet LeaveMasterExport(int companyId, int siteCode);
        Task<string> PostDeleteLeave(int leavemasterid);
    }
}
