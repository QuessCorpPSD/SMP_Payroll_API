using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay_Web_Domain.Models.Increment;
//using static QZone.DTo.Models.Common.ActivationLwd;

namespace QPay.BAL.IRepository.PayrollInput
{
    public interface IIncrementRepository
    {
        DataSet GetEmployeeIncrement(int companyId, int payPeriodId, int InputType, int MapNameId, string schema);
        Task<IncrementResponse> UploadIncrementData(IFormFile file, [FromForm] string User,
           [FromForm] string companyCode, [FromForm] int companyId, [FromForm] int InputType, string schema);
    }
}
