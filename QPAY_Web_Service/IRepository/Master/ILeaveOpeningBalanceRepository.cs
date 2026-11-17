using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static QPay.UI_Domain.Models.Master.LeaveOpening;

namespace QPay.BAL.IRepository.Master
{
    public interface ILeaveOpeningBalanceRepository
    {
        Task<DataSet> GetLeaveOpeingBalance(string CompanyId, string Sitename);
        Task<DataSet> GetLeaveOpeingBalanceTemplate(LeaveOpeningTemplateRequest leave);
        Task<LeaveOpeningResponse> ImportLeaveOpeingBalance(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string Fromdate, [FromForm] string Todate, [FromForm] string CreatedBy);
    }
}
