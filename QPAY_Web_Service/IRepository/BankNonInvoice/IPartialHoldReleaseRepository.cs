using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.IRepository.BankNonInvoice
{
    public interface IPartialHoldReleaseRepository
    {
        Task<PartialEmpSalaryResponse> PartialHold(IFormFile file, string User);

        Task<PartialEmpSalaryResponse> PartialHoldRelease(IFormFile file, string User);
    }
}
