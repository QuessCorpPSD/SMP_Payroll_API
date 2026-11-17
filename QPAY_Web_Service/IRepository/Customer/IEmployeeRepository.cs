using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface IEmployeeRepository
    {
        Task<DataSet> SearchDetails(int? Company, string? Employee_code, string schema);
        Task<DataSet> GetEmployeeBilltoRate(int? companyId, string schema);
        Task<DataSet> ExportToExcel(int? CompanyId, string EmployeeId, int? EActive, string schema);

        Task<List<CategoryUI>> GetCategory(string schema);
        Task<DataSet> GetWorkpass(string schema);
        Task<DataSet> Department(int? companyId, string schema);
        Task<DataSet> Designation(int? companyId, string schema);
        Task<DataSet> BillingDesignation(int? companyId, string schema);
        Task<DataSet> Costcentermapping(int? companyId, string schema);
        Task<DataSet> Band(int? companyId, string schema);
        Task<DataSet> GetCostCenter(int? companyId, string schema);
        Task<DataSet> GroupMater(int? companyId, string schema);
        Task<DataSet> GetAllPayPeriodByCompanyID(int? companyId, string schema);

        Task<DataSet> SearchBank(string schema);
        Task<DataSet> GetRole(string schema);
        Task<DataSet> GetEmploymentType(string schema);
        Task<EmployeeApiResponse> PostEmployeeUpload(string xmlString, string userId, string schema);
        Task<EmployeeApiResponse> PostEmployeeSalaryUpload(string xmlString, string userId, string schema);

        Task<DataSet> Create(EmployeeRequest request, string schema);
        Task<DataSet> BankCreate(EmployeeBankRequest request, string schema);
        Task<DataSet> InformationCreate(EmployeeInformationRequest request, string schema);
        Task<DataSet> ContactCreate(EmployeeContactRequest request, string schema);
        Task<DataSet> PersonalCreate(EmployeePersonalRequest request, string schema);
        Task<DataSet> PreviousCreate(EmployeePreviousRequest request, string schema);

        Task<DataSet> SearchSalary(int employeeId, string schema);
        Task<List<LegalEntityUI>> GetLegalEntity(string schema);

    }
}
