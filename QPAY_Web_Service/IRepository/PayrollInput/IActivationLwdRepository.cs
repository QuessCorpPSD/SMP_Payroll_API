using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI_Domain.Models.ActivationLwd;

namespace QPay.BAL.IRepository.PayrollInput
{
    public interface IActivationLwdRepository
    {
        Task<object> GetEmployeeActivationLwd(string companyId, string flag);        
        Task<ActivationResponse> UploadEmployeeActivation(IFormFile file, [FromForm] string User,
            [FromForm] string COMPANY_CODE, [FromForm] string FLAG);            
        Task<ActivationResponse> UploadEmployeeLWD(IFormFile file, [FromForm] string User,
            [FromForm] string COMPANY_CODE, [FromForm] string FLAG);
        
    }
}
