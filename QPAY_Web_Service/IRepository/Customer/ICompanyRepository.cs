using QPay.UI.Customer;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Customer.Company;

namespace QPay.BAL.IRepository.Customer
{
    public interface ICompanyRepository
    {
        Task<DataTable> Search(string action, int? companyId, string xml, string schema);
        Task<DataSet> View(string action, int? companyId, string xml, string schema);
        Task<DataSet> ExportToExcel(string action, int? companyId, string xml, string schema);
        Task<CompanyDetails> masters(string schema);

        Task<DataSet> Create(CompanyCreateRequest request, string schema);
        Task<DataSet> Update(CompanyUpdateRequestPayload request, string schema);
        Task<DataSet> DeleteCompany(CompanyDeleteRequest request, string schema);
        Task<DataSet> GetBussinessunitLocation(int? BusinessUnitId, string schema);
        Task<DataSet> GetCityBasedonState(int? Stateid, string schema);

        Task<DataSet> GetStatebasedoncity(int? cityid, string schema);

        Task<DataSet> GetInvoiceFormat(string schema);
        Task<DataSet> GetReimbInvoiceFormat(string schema);
        Task<DataSet> GetPortalPayslipFormat(string schema);

    }
}
