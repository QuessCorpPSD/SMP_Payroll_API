using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.Repository.Customer
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(int? Company, string? Employee_code, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company,
                ["@Employee_code"] = Employee_code,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllEmployeeByPaging_New_api", parameters, 1500);
        }

        public async Task<DataSet> GetEmployeeBilltoRate(int? companyId,string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".SP_Get_BilltoRate_EmployeeCode", parameters, 1500);
        }


        public async Task<DataSet> ExportToExcel(int? CompanyId, string EmployeeId, int? EActive, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = CompanyId,
                ["@Employee_Id"] = EmployeeId,
                ["@EActive"] = EActive
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllEmployeeDetailsExportToExcel", parameters, 1500);
        }

        public async Task<List<CategoryUI>> GetCategory(string schema)
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_GetCategory_CPF", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CategoryUI>>(res) ?? new List<CategoryUI>();
            }

            return new List<CategoryUI>();
        }

        public async Task<DataSet> GetWorkpass(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_Get_Workpass_Master", parameters, 1500);
        }

        public async Task<DataSet> Department(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id,
                ["@Department_Id"] = 0
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetDepartmentDetails", parameters, 1500);
        }

        public async Task<DataSet> Designation(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetDesignationDetails", parameters, 1500);
        }

        public async Task<DataSet> BillingDesignation(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetBillingDesignationDetails", parameters, 1500);
        }

        public async Task<DataSet> Costcentermapping(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetCostCenterMappingForEmployee", parameters, 1500);
        }

        public async Task<DataSet> Band(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetBandDetails", parameters, 1500);
        }

        public async Task<DataSet> GetCostCenter(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetCostCenter", parameters, 1500);
        }

        public async Task<DataSet> GroupMater(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetGroupMasterDetails", parameters, 1500);
        }

        public async Task<DataSet> GetAllPayPeriodByCompanyID(int? Company_id, string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllPaySequenceByCompanyID", parameters, 1500);
        }


        public async Task<DataSet> SearchBank(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@BankName"] = "",
                ["@BranchName"] = "",
                ["@Bank_Id"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetBankDetails", parameters, 1500);
        }


        public async Task<DataSet> GetRole(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetRole", parameters, 1500);
        }


        public async Task<DataSet> GetEmploymentType(string schema)
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".spEmploymentTypedroprown", parameters, 1500);
        }

        public async Task<EmployeeApiResponse> PostEmployeeUpload(string xmlString, string userId, string schema)
        {
            EmployeeApiResponse employeeDetails = new EmployeeApiResponse();

            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync(schema + ".SpImportEmployee", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        employeeDetails.response = message;
                    }
                    else
                    {
                        employeeDetails.response = "Failed to import.";
                        employeeDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    employeeDetails.response = "Error while processing response.";
                }
            }
            else
            {
                employeeDetails.response = "Failed";
            }

            return employeeDetails;

        }

        public async Task<EmployeeApiResponse> PostEmployeeSalaryUpload(string xmlString, string userId, string schema)
        {
            EmployeeApiResponse employeeDetails = new EmployeeApiResponse();
            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync(schema + ".Proc_Upload_NewJoineeSalary", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        employeeDetails.response = message;
                    }
                    else
                    {
                        employeeDetails.response = "Failed to import.";
                        employeeDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    employeeDetails.response = "Error while processing response.";
                }
            }
            else
            {
                employeeDetails.response = "Failed";
            }

            return employeeDetails;

        }


        public async Task<DataSet> Create(EmployeeRequest items, string schema)
        {
            var EmpResponse = new EmployeeResponse();
            EmpResponse.EmployeeDetails = new EmployeeWithDetails[1];
            EmpResponse.EmployeeDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Employee",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateEmployee", parameters);
        }


        public async Task<DataSet> BankCreate(EmployeeBankRequest items, string schema)
        {
            var EmpResponse = new EmployeeBankDetailResponse();
            EmpResponse.EmployeeBankDetails = new EmployeeBankDetail[1];
            EmpResponse.EmployeeBankDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeBankDetailResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Bank",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> InformationCreate(EmployeeInformationRequest items, string schema)
        {
            var EmpResponse = new EmployeeInformationResponse();
            EmpResponse.EmployeeInformationDetails = new EmployeeInformation[1];
            EmpResponse.EmployeeInformationDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeInformationResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Information",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> ContactCreate(EmployeeContactRequest items, string schema)
        {
            var EmpResponse = new EmployeeContactResponse();
            EmpResponse.EmployeeContactDetails = new EmployeeContactDetail[1];
            EmpResponse.EmployeeContactDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeContactResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Contact",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> PersonalCreate(EmployeePersonalRequest items, string schema)
        {
            var EmpResponse = new EmployeePersonalResponse();
            EmpResponse.EmployeePersonalDetails = new EmployeePersonalDetail[1];
            EmpResponse.EmployeePersonalDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeePersonalResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Personal",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> PreviousCreate(EmployeePreviousRequest items, string schema)
        {
            var parentdata = GenericSerializer<EmployeePreviousEmployment>.Serialize(items.detail);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Previous",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> SearchSalary(int employeeId, string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Employee_Id"] = employeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetEmployeeSalaryInformation", parameters);
        }

        public async Task<List<LegalEntityUI>> GetLegalEntity(string schema)
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync(schema + ".SP_Get_LegalEntity", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<LegalEntityUI>>(res) ?? new List<LegalEntityUI>();
            }
            return new List<LegalEntityUI>();
        }

    }
}
