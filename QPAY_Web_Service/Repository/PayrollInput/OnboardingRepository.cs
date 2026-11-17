using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using QPay.BAL.IRepository.PayrollInput;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.IRepository.Repository.PayrollInput
{
    public class OnboardingRepository : IOnboardingRepository
    {
        private readonly DbRepository _dbRepository;

        public OnboardingRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<Onboarding>> GetAllOnboardingDetails(string companyId, string? payPeriod, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyCode", companyId);
            parameters.Add("@PayPeriod", payPeriod);

            var res = await this._dbRepository.GetItemsAsync(schema+".Proc_Onboarding_data", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Onboarding>>(res) ?? new List<Onboarding>();
            }

            return new List<Onboarding>();
        }
        public async Task<DataSet> GetNewJoineeTemplate (int companyId, int payPeriodId, int flag, int mapNameId, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Flag"] = flag,
                ["@Employee_Id"] = 0,
                ["@Company_Id"] = companyId,
                ["@PayPeriod_Id"] = payPeriodId,
                ["@MapNameId"] = mapNameId,
                ["@ISFANDF"] = 0,
                ["@InputType"] = 1,
                ["@LotNumber"] = 0
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".Proc_view_Excel_Format", parameters, 1500);
        }


        public async Task<string> MoveToQpay(string xmlString, int companyId, string payPeriod, int payPeriodId, string userId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@xml", xmlString);
            parameters.Add("@User", userId);
            parameters.Add("@PayPeriod", payPeriod);
            parameters.Add("@Payperiod_Id", payPeriodId);
            parameters.Add("@Company_Id", companyId);


            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_Transfer_Employee", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<string> PostValidateOfferId(string xmlString, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OFFERXML", xmlString);

            var res = await this._dbRepository.GetItemsAsync(schema + ".USP_OFFERID_CheckPoints", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> PostRollbackOfferId(string xmlString, string userId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@XMLDATA", xmlString);
            parameters.Add("@CREATEDBY", userId);

            var res = await this._dbRepository.GetItemsAsync(schema + ".USP_UPLOAD_OFFERID_ROLLBACK", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<string> PostNewJoineeData(string xmlString, string companyCode, int companyId, int payPeriodId, string filePath, string userId, string schema)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@XML", xmlString);
            parameters.Add("@user", userId);
            parameters.Add("@COMPANY_CODE", companyCode);
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Payperiod_Id", payPeriodId);
            parameters.Add("@InputType", "1");
            parameters.Add("@LotNumber", "0");
            parameters.Add("@Filepath", filePath);


            var res = await this._dbRepository.GetItemsAsync(schema + ".PROC_BULK_UPLOAD_HARBOUR_EMPLOYEE", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public List<PayperiodDD> GetCurrentPayperiod(int companyId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = this._dbRepository.GetItemsAsync(schema + ".Proc_GetCurrentPayperiod", parameters).Result;
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }
        public async Task<string> PostAttendanceData(string xmlString, int companyId, int payPeriodId, string filePath, string xmlString2, string userId, string schema)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@payperiod_Id", payPeriodId);
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@User", userId);
            parameters.Add("@ISFANDF", "0");
            parameters.Add("@xml", xmlString);
            parameters.Add("@InputType", "1");
            parameters.Add("@LotNumber", "0");
            parameters.Add("@XML1", xmlString2);
            parameters.Add("@FilePath", filePath);



            var res = await this._dbRepository.GetItemsAsync(schema + ".PROC_BULK_UPLOAD_ATTENDANCE_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public async Task<string> VerifyAttendanceHeaders(string xmlString, int companyId, int payPeriodId, string filePath, string schema)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@xML", xmlString);

            var res = await this._dbRepository.GetItemsAsync(schema + ".USP_Attendance_Upload_IsValidHeaders_New", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> PostOneTimeInputData(string xmlString, int companyId, int payPeriodId, string filePath, string userId, string schema)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@payperiod_Id", payPeriodId);
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@User", userId);
            parameters.Add("@xml", xmlString);
            parameters.Add("@Flag", '1');
            parameters.Add("@InputType", "1");
            parameters.Add("@LotNumber", "0");
            parameters.Add("@FilePath", filePath);



            var res = await this._dbRepository.GetItemsAsync(schema + ".PROC_BULK_UPLOAD_ONE_TIME_INPUT_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public async Task<List<FinalSubmission>> GetAllFinalSubmitDetails(int companyId, int payPeriodId, string Action, string userId, string schema)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@InputLotNumber", '0');
            parameters.Add("@Action", Action);
            parameters.Add("@userid", userId);


            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_Manage_FinalSubmit_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<FinalSubmission>>(res) ?? new List<FinalSubmission>();
            }

            return new List<FinalSubmission>();
        }
        public async Task<string> PostFinalSubmission(int companyId, int payPeriodId, string LotNos, string userId, string schema)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@InputLotNumber", LotNos);
            parameters.Add("@Action", "Edit");
            parameters.Add("@userid", userId);

            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_Manage_FinalSubmit_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public string GetRegisterFilename(int companyId, int payPeriodId, int lotNumber, string inputType, int flag, string schema)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@InputLotNumber", lotNumber);
            parameters.Add("@Input_type", inputType);
            parameters.Add("@LoginUser", "1");
            parameters.Add("@FileName", "");
            parameters.Add("@Flag", flag);

            var res = this._dbRepository.GetItemsAsync(schema + ".Proc_Manage_Attached_Salary_Register", parameters).Result;

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<DataSet> GetNewJoineeEmployeeId(int companyId, string payPeriod, int lotNumber, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@PayPeriod"] =  payPeriod,
                ["@InputlotNo"] = lotNumber
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllEmployeeDetails_QZone", parameters, 1500);
        }

        public async Task<DataSet> GetConsolidatePayRegister(int companyId, int payPeriodId, string lotNumber, string schema)
        { 
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
                ["@Lot_No"] = lotNumber
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_PayRegister_MultipleLot", parameters, 1500);
        }
    }
}
