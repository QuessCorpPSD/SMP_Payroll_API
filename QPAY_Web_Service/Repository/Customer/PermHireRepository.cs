using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Customer.PermHire;

namespace QPay.BAL.Repository.Customer
{
    public class PermHireRepository : IPermHireRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public PermHireRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetServiceChargeType(ServiceChargeTypeRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = request.flag,
                ["@QRS_Service_Charge_Type"] = request.QRS_Service_Charge_Type
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageQRSService", parameters, 1500);
        }

        public async Task<PermHireResponse> ServiceChargeTypeCreate([FromBody] ServiceChargeTypeDto request)
        {
            PermHireResponse permhireDetails = new PermHireResponse();

            if (request == null || request.Rows == null || !request.Rows.Any())
            {
                permhireDetails.response = "Invalid Service Charge Type request.";
            }

            var xmlInput = BuildServiceChargeTypeXml(request);

            string storeProcedure = "Proc_ManageQRSService";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@Action", request.flag);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<PermHireResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        permhireDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        permhireDetails.response = message;
                    }
                    else
                    {
                        permhireDetails.response = "Failed to Save.";
                        permhireDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    permhireDetails.response = "Error while processing response.";
                }
            }
            else
            {
                permhireDetails.response = "Failed";
            }


            return permhireDetails;
        }

        private string BuildServiceChargeTypeXml(ServiceChargeTypeDto request)
        {
            var sb = new StringBuilder();
            sb.Append("<QRSSMaster>");

            foreach (var row in request.Rows)
            {
                sb.Append("<QRSSMasterDetails>");
                sb.AppendFormat("<QRS_Service_Charge_Type_Id>{0}</QRS_Service_Charge_Type_Id>", row.QRS_Service_Charge_Type_Id);
                sb.AppendFormat("<QRS_Service_Charge_Type>{0}</QRS_Service_Charge_Type>", row.QRS_Service_Charge_Type);
                sb.Append("</QRSSMasterDetails>");
            }

            sb.Append("</QRSSMaster>");
            return sb.ToString();
        }

        public async Task<DataSet> GetJobCategorySearch(JobCategoryRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = request.flag,
                ["@JOB_Category"] = request.JOB_Category
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageJobcategory", parameters, 1500);
        }

        public async Task<PermHireResponse> JobCategoryCreate([FromBody] JobCategoryDto request)
        {
            PermHireResponse permhireDetails = new PermHireResponse();

            if (request == null || request.Rows == null || !request.Rows.Any())
            {
                permhireDetails.response = "Invalid Service Charge Type request.";
            }

            var xmlInput = BuildJobCategoryXml(request);

            string storeProcedure = "Proc_ManageJobcategory";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@Action", request.flag);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<PermHireResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        permhireDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        permhireDetails.response = message;
                    }
                    else
                    {
                        permhireDetails.response = "Failed to Save.";
                        permhireDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    permhireDetails.response = "Error while processing response.";
                }
            }
            else
            {
                permhireDetails.response = "Failed";
            }


            return permhireDetails;
        }

        private string BuildJobCategoryXml(JobCategoryDto request)
        {
            var sb = new StringBuilder();
            sb.Append("<JobcategoryMaster>");

            foreach (var row in request.Rows)
            {
                sb.Append("<JobcategoryMasterDetails>");
                sb.AppendFormat("<JOB_Category_ID>{0}</JOB_Category_ID>", row.JOB_Category_ID);
                sb.AppendFormat("<JOB_Category>{0}</JOB_Category>", row.JOB_Category);
                sb.Append("</JobcategoryMasterDetails>");
            }

            sb.Append("</JobcategoryMaster>");
            return sb.ToString();
        }

        public async Task<DataSet> GetJobCategory()
        {
            var parameters = new Dictionary<string, object?>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_GetQRS_JOB_Category", parameters, 1500);
        }

        public async Task<DataSet> GetJobSubCategorySearch(int CompanyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = CompanyId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllQRSJobSubCategory", parameters, 1500);
        }

        public async Task<DataSet> GetJobSubCategoryExport(int CompanyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllQRSSubJobDetailExportToExcel", parameters, 1500);
        }

        public async Task<PermHireResponse> JobSubCategoryCreate([FromBody] JobSubCategoryDto request)
        {
            PermHireResponse permhireDetails = new PermHireResponse();

            if (request == null || request.Rows == null || !request.Rows.Any())
            {
                permhireDetails.response = "Invalid Service Charge Type request.";
            }

            var xmlInput = BuildJobSubCategoryXml(request);

            string storeProcedure = "sp_CreateUpdateQRSJobSubCategory";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@mode", request.flag);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<PermHireResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("successfully"))
                    {
                        permhireDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) && message.Contains("successfully"))
                    {
                        permhireDetails.response = message;
                    }
                    else
                    {
                        permhireDetails.response = "Failed to Save.";
                        permhireDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    permhireDetails.response = "Error while processing response.";
                }
            }
            else
            {
                permhireDetails.response = "Failed";
            }


            return permhireDetails;
        }

        private string BuildJobSubCategoryXml(JobSubCategoryDto request)
        {
            var sb = new StringBuilder();
            sb.Append("<QRSJOBSUBCategoryDetails>");

            foreach (var row in request.Rows)
            {
                sb.Append("<QRSJOBSUBCategory>");
                sb.AppendFormat("<JOB_SUB_Category_ID>{0}</JOB_SUB_Category_ID>", row.JOB_SUB_Category_ID);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", row.Company_Id);
                sb.AppendFormat("<JOB_Category_ID>{0}</JOB_Category_ID>", row.JOB_Category_ID);
                sb.AppendFormat("<JOB_SUB_Category>{0}</JOB_SUB_Category>", row.JOB_SUB_Category);
                sb.Append("</QRSJOBSUBCategory>");
            }

            sb.Append("</QRSJOBSUBCategoryDetails>");
            return sb.ToString();
        }

        public async Task<DataSet> GetPermHireMasterSearch(PermHireMasterRequest request)
        {
            if (request.mode == "Search")
            {
                var parameters = new Dictionary<string, object?>
                {
                    ["@Companycode"] = request.companyId == 0 ? DBNull.Value : request.companyId,
                    ["@Status"] = string.IsNullOrWhiteSpace(request.status) ? DBNull.Value : request.status
                };
                return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetQRSDetails", parameters, 1500);
            }
            else
            {
                var parameters = new Dictionary<string, object?>
                {
                    ["@CompanyId"] = request.companyId == 0 ? DBNull.Value : request.companyId,
                    ["@Status"] = string.IsNullOrWhiteSpace(request.status) ? DBNull.Value : request.status
                };
                return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllQRSDetailExportToExcel", parameters, 1500);
            }
        }

        public async Task<PermHireResponse> PermHireMasterApproveReject([FromBody] PermHireMasterDto request)
        {
            PermHireResponse permhireDetails = new PermHireResponse();

            if (request == null || request.Rows == null || !request.Rows.Any())
            {
                permhireDetails.response = "Invalid Service Charge Type request.";
            }

            var xmlInput = PermHireMasterXml(request);

            string storeProcedure = "sp_CreateUpdateQRS";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@mode", "");
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<PermHireResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.ToLower().Contains("successfully"))
                    {
                        permhireDetails.response = message;
                    }
                    else
                    {
                        permhireDetails.response = "Failed to Save.";
                        permhireDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    permhireDetails.response = "Error while processing response.";
                }
            }
            else
            {
                permhireDetails.response = "Failed";
            }


            return permhireDetails;
        }

        private string XmlEscape(object value)
        {
            return SecurityElement.Escape(value?.ToString() ?? string.Empty);
        }

        private string PermHireMasterXml(PermHireMasterDto request)
        {
            var sb = new StringBuilder();
            sb.Append("<QRSDetails>");

            foreach (var row in request.Rows)
            {
                sb.Append("<QRS>");

                sb.AppendFormat("<Id>{0}</Id>", XmlEscape(row.Id));
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", XmlEscape(row.Company_Id));
                sb.AppendFormat("<LOCATION>{0}</LOCATION>", XmlEscape(row.LOCATION));
                sb.AppendFormat("<REQ_ID>{0}</REQ_ID>", XmlEscape(row.REQ_ID));
                sb.AppendFormat("<REF_ID>{0}</REF_ID>", XmlEscape(row.REF_ID));
                sb.AppendFormat("<CAND_ID>{0}</CAND_ID>", XmlEscape(row.CAND_ID));
                sb.AppendFormat("<CAND_NAME>{0}</CAND_NAME>", XmlEscape(row.CAND_NAME));
                sb.AppendFormat("<DESIGNATION>{0}</DESIGNATION>", XmlEscape(row.DESIGNATION));
                sb.AppendFormat("<DOJ>{0}</DOJ>", XmlEscape(row.DOJ));
                sb.AppendFormat("<VERTICAL>{0}</VERTICAL>", XmlEscape(row.VERTICAL));
                sb.AppendFormat("<VH>{0}</VH>", XmlEscape(row.VH));
                sb.AppendFormat("<CTC>{0}</CTC>", XmlEscape(row.CTC));
                sb.AppendFormat("<BILLABLE_CTC>{0}</BILLABLE_CTC>", XmlEscape(row.BILLABLE_CTC));
                sb.AppendFormat("<CONTRACT_TYPE>{0}</CONTRACT_TYPE>", XmlEscape(row.CONTRACT_TYPE));
                sb.AppendFormat("<BRANCH_CODE>{0}</BRANCH_CODE>", XmlEscape(row.BRANCH_CODE));
                sb.AppendFormat("<INVOICE_NO>{0}</INVOICE_NO>", XmlEscape(row.INVOICE_NO));
                sb.AppendFormat("<INVOICE_STATE>{0}</INVOICE_STATE>", XmlEscape(row.INVOICE_STATE));
                sb.AppendFormat("<TOT_INVOICE_AMOUNT>{0}</TOT_INVOICE_AMOUNT>", XmlEscape(row.TOT_INVOICE_AMOUNT));
                sb.AppendFormat("<GST_NO>{0}</GST_NO>", XmlEscape(row.GST_NO));
                sb.AppendFormat("<STATUS>{0}</STATUS>", XmlEscape(row.STATUS));
                sb.AppendFormat("<CITY>{0}</CITY>", XmlEscape(row.CITY));
                sb.AppendFormat("<DOB>{0}</DOB>", XmlEscape(row.DOB));
                sb.AppendFormat("<GENDER>{0}</GENDER>", XmlEscape(row.GENDER));
                sb.AppendFormat("<PAN>{0}</PAN>", XmlEscape(row.PAN));
                sb.AppendFormat("<AADHAR>{0}</AADHAR>", XmlEscape(row.AADHAR));
                sb.AppendFormat("<MOBILE_NUMBER>{0}</MOBILE_NUMBER>", XmlEscape(row.MOBILE_NUMBER));
                sb.AppendFormat("<STATE>{0}</STATE>", XmlEscape(row.STATE));
                sb.AppendFormat("<JOB_CATEGORY>{0}</JOB_CATEGORY>", XmlEscape(row.JOB_CATEGORY));
                sb.AppendFormat("<JOB_SUB_CATEGORY>{0}</JOB_SUB_CATEGORY>", XmlEscape(row.JOB_SUB_CATEGORY));
                sb.AppendFormat("<PLACEMENT_TYPE>{0}</PLACEMENT_TYPE>", XmlEscape(row.PLACEMENT_TYPE));
                sb.AppendFormat("<YEAR_EXPERIENCE>{0}</YEAR_EXPERIENCE>", XmlEscape(row.YEAR_EXPERIENCE));
                sb.AppendFormat("<JOINING_DAYS>{0}</JOINING_DAYS>", XmlEscape(row.JOINING_DAYS));
                sb.AppendFormat("<Approved_Rejected_ByName>{0}</Approved_Rejected_ByName>", XmlEscape(row.Approved_Rejected_ByName));
                sb.AppendFormat("<Approved_Rejected_Remarks>{0}</Approved_Rejected_Remarks>", XmlEscape(row.Approved_Rejected_Remarks));
                sb.AppendFormat("<Approval_Status>{0}</Approval_Status>", XmlEscape(row.Approval_Status));
                sb.AppendFormat("<JOBCode>{0}</JOBCode>", XmlEscape(row.JOBCode));
                sb.AppendFormat("<Consultant>{0}</Consultant>", XmlEscape(row.Consultant));
                sb.AppendFormat("<Offer_Approval_Raised_by_Recruiter>{0}</Offer_Approval_Raised_by_Recruiter>", XmlEscape(row.Offer_Approval_Raised_by_Recruiter));
                sb.AppendFormat("<Requested_on>{0}</Requested_on>", XmlEscape(row.Requested_on));
                sb.AppendFormat("<Recruiters_Employee_ID>{0}</Recruiters_Employee_ID>", XmlEscape(row.Recruiters_Employee_ID));
                sb.AppendFormat("<EntityID>{0}</EntityID>", XmlEscape(row.EntityID));
                sb.AppendFormat("<Organisation_Head>{0}</Organisation_Head>", XmlEscape(row.Organisation_Head));
                sb.AppendFormat("<UPDATED_DATE>{0}</UPDATED_DATE>", XmlEscape(row.UPDATED_DATE));
                sb.AppendFormat("<Input_Number>{0}</Input_Number>", XmlEscape(row.Input_Number));
                sb.AppendFormat("<IS_PO_Applicable>{0}</IS_PO_Applicable>", XmlEscape(row.IS_PO_Applicable));
                sb.AppendFormat("<PO_NUMBER>{0}</PO_NUMBER>", XmlEscape(row.PO_NUMBER));

                sb.Append("</QRS>");
            }

            sb.Append("</QRSDetails>");

            return sb.ToString();
        }
    }
}
