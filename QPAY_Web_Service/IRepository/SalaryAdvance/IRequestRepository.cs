using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.SalaryAdvance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.SalaryAdvance
{
    public interface IRequestRepository
    {
        Task<DataSet> Search(int? CompanyId, int? PayPeriod_Id, int? employeeid);
        Task<DataSet> GetTemplate();

        Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy,
         [FromForm] string CompanyCode, [FromForm] int Payperiod_id, [FromForm] string Payperiod);

    }
}
