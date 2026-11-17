using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Invoice.InvoiceInitiationAgainstProfoma;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IInvoiceInitiationAgainstProfomaRepository
    {
        Task<InvoiceInitiationAgainstProfomaResponse> ProfomaImport(IFormFile file, [FromForm] int createdBy);
        Task<DataSet> GetProfoma(int CompanyId, int PayperiodId,string flag);
        Task<InvoiceInitiationAgainstProfomaResponse> Initiate([FromBody] InitiateRequestDto request);

    }
}
