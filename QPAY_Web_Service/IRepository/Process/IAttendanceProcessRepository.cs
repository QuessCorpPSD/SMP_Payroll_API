using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;
using static QPay_Web_Domain.Models.Timesheet;

namespace QPay.BAL.IRepository.Process
{
    public interface IAttendanceProcessRepository
    {
        Task<DataSet> SearchDetails(SearchRequest searchRequest, string schema);
        Task<DataSet> ExporttoExcel(ExporttoExcelRequest exporttoExcelRequest, string schema);
        Task<AttendanceProcessResponse> ImportAttendnace(IFormFile file, [FromForm] string User, string schema);
    }
}
