using DocumentFormat.OpenXml.Bibliography;
using QPay.UI.Common;
using QPay.UI.Models;

namespace QPay.IRepository.iRepository.Common

{
    public interface ICommonRepository
    {
        Task<List<CompanyPicker>> GetallCompanyCodes(int userId, string schema);
        Task<List<PayperiodDD>> GetAllPayperiod(int companyId, string schema);
        List<PayperiodDD> GetCurrentPayperiod(int companyId, string schema);
        Task<List<MapnameDD>> GetMapNamebyCompany(int companyId, string schema);
        Task<List<InputTypeDD>> GetAllInputType(string schema);
        Task<List<PSDStatus>> GetLotwisePSDStatus(int companyId, int payPeriodId, int lotNumber, string Payroll_Input_Type, string schema);
        Task<List<Site>> GetSitesByCompanyId(int companyId, string schema);
        Task<List<UI.Models.City>> GetCityByCompanyCode(string CompanyCode,int Group_Id, string schema);
        Task<List<AllPayperiod>> GetPayPeriod(string schema);
        Task<List<Paycodes>> GetPaycodes(string schema);
        Task<List<StateUI>> GetAllState(string schema);
        Task<List<CityUI>> GetCityByStateId(int stateId, string schema);
        Task<List<Paycodes>> GetMultiCommercialPaycodes(string schema);
        Task<List<PayperiodDD>> GetSalaryReleasePayperiod(int companyId, string Flag, string schema);
        Task<List<CompanyPicker>> GetProfomaCompany(int userId, string schema);
    }
}
