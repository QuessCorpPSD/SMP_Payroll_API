using Microsoft.AspNetCore.Mvc;
using QPay.UI_Domain.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Master
{
    public interface IInvoiceRuleRepository
    {
        Task<List<InvoiceRule>> GetAllInvoiceRule(int? companyId, string? siteId);
        Task<string> PostAddInvoiceRule(InvoiceRuleAdd invoiceRuleAdd);
        Task<string> PostEditInvoiceRule([FromBody] InvoiceRuleEdit invoiceruleEdit);
        Task<string> PostDeleteInvoiceRule(int invoicingRulesID);
        DataSet GetInvoiceRuleTemplate(int companyId, string siteName);
        Task<string> PostInvoiceRuleUpload(string xmlString, string userId);
        DataSet InvoiceRuleExport(int companyId, int siteCode);

    }
}
