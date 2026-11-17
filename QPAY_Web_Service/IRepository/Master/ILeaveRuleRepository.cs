using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI_Domain.Models.Master.LeaveRule;


namespace QPay.BAL.IRepository.Master
{
    public interface ILeaveRuleRepository
    {
        Task<DataSet> GetLeaveRuleCompanywise(int CompanyId, int SiteId,string State);
        Task<DataSet> GetLeaveRuleTemplate();
        Task<LeaveRuleResponse> UploadLeaveRule(IFormFile file, [FromForm] string User);
        Task<List<LeaveTypes>> GetLeaveType(string CompanyId);
        Task<LeaveRuleResponse> SaveUpdateDeleteLeaveRule([FromBody] LeaveRuleRequest request);
    }
}
