using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models;
using QPay_Web_Domain.Models;
using static QPay_Web_Domain.Models.Timesheet;

namespace QPay.BAL.IRepository.PayrollInput
{
    public interface ITimesheetRepository
    {
        Task<DataSet> GetEmployeeTimesheetDaywise(string CompanyCode, int GroupName, string Empid,
            int PayPriod_Id, int City_Id);
        Task<DataSet> GetEmployeeTimesheetDaywiseDownload(string CompanyCode, int GroupName, string Empid,
            int PayPriod_Id, int City_Id);
        Task<TimesheetResponse> UploadDailyTimesheet(IFormFile file, [FromForm] string User,
          [FromForm] string CompanyCode, [FromForm] int SiteID, [FromForm] int Payperiod);
        Task<TimesheetResponse> UploadDocumentSingleMulitiple(IFormFile file, [FromForm] string User,
 [FromForm] string Employeeid, [FromForm] string CompanyCode, [FromForm] int Site_ID, [FromForm] int Payperiod_ID,
 [FromForm] string Payperiod, string fullUrl);
        Task<List<TimesheetAttachment>> GetTimesheetAttachment(string CompanyCode, int Site_ID,
    string Employee_Code, string Payperiod);
        Task<TimesheetResponse> SaveTimesheet([FromBody] TimesheetRequestDto request);
        Task<string> PostAttendanceData(string xmlString, int companyId, int payPeriodId, string filePath, string xmlString2, string userId);
        Task<string> VerifyAttendanceHeaders(string xmlString, int companyId, int payPeriodId, string filePath);
        Task<List<Unseize>> GetUnseizeData(string CompanyCode, int PayPriod_Id, int GroupName, int City_Id, string Empid);
        Task<string> PostUnseize(string xmlString, int companyId, int payPeriodId, int siteCode, string userId);
        Task<List<Attachment>> GetUnseizeAttachment(string companyCode, int siteId, string empCode, string payPeriod);
        Task<DataSet> GetTimesheetDataforAudit(string CompanyCode, int GroupName, string Empid,
           int PayPriod_Id, int City_Id);

        Task<string> RejectTimesheet(string xmlString, string companyCode, int? payPeriodId, int? userId);
        Task<DataSet> GetAttendanceReport(string CompanyCode, string GroupName, int PayPriod_Id);
    }
}
