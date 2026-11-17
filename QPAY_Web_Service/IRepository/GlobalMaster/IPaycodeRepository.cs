using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.ActivationLwd;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IPaycodeRepository
    {
        Task<DataSet> Search(string strPayCode, int? intPayTypeId, int? IsTaxable, int? PayId, string schema);
        Task<DataSet> Create(PaycodeCreateParams _params, string schema);
        Task<DataSet> GetPageType(string schema);
        Task<string> GetPayType(string schema);

        //Task<ActivationResponse> UploadEmployeeActivation(IFormFile file, [FromForm] string User,
        //    [FromForm] string COMPANY_CODE, [FromForm] string FLAG);            
        //Task<ActivationResponse> UploadEmployeeLWD(IFormFile file, [FromForm] string User,
        //    [FromForm] string COMPANY_CODE, [FromForm] string FLAG);

    }
}
