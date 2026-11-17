using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.PayrollInput
{
    public interface IOnboardingRepository
    {
        Task<List<Onboarding>> GetAllOnboardingDetails(string companyId, string? payPeriod, string schema);
        Task<DataSet> GetNewJoineeTemplate(int companyId, int payPeriodId, int flag, int mapNameId, string schema);
        Task<string> MoveToQpay(string xmlString, int companyId, string payPeriod, int payPeriodId, string userId, string schema);
        Task<string> PostValidateOfferId(string xmlString, string schema);
        Task<string> PostRollbackOfferId(string xmlString, string userId, string schema);
        Task<string> PostNewJoineeData(string xmlString, string companyCode, int companyId, int payPeriodId, string filePath, string userId, string schema);
        Task<string> PostAttendanceData(string xmlString, int companyId, int payPeriodId, string filePath, string xmlString2, string userId, string schema);
        Task<string> VerifyAttendanceHeaders(string xmlString, int companyId, int payPeriodId, string filePath, string schema);
        List<PayperiodDD> GetCurrentPayperiod(int companyId, string schema);
        Task<string> PostOneTimeInputData(string xmlString, int companyId, int payPeriodId, string filePath, string userId, string schema);
        Task<List<FinalSubmission>> GetAllFinalSubmitDetails( int companyId, int payPeriodId, string Action, string userId, string schema);
        string GetRegisterFilename(int companyId, int payPeriodId, int lotNumber, string inputType, int flag, string schema);
        Task<string> PostFinalSubmission(int companyId, int payPeriodId, string LotNos, string userId, string schema);
        Task<DataSet> GetNewJoineeEmployeeId(int companyId, string payPeriod, int lotNumber, string schema);
        Task<DataSet> GetConsolidatePayRegister(int companyId, int payPeriodId, string lotNumber, string schema);


    }
}
