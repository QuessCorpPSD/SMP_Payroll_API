using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Customer
{
    public class PermHire
    {

        public class PermHireResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class PermHireResponseModel
        {
            public string? Result { get; set; }
            public string? Error_Message { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public class ServiceChargeTypeRequest
        {
            public string? flag { get; set; }
            public string? QRS_Service_Charge_Type { get; set; }
        }

        public class ServiceChargeTypeRowDto
        {
            public int QRS_Service_Charge_Type_Id { get; set; }
            public string QRS_Service_Charge_Type { get; set; } = string.Empty;

        }

        public class ServiceChargeTypeDto
        {
            public string flag { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public List<ServiceChargeTypeRowDto> Rows { get; set; } = new();
        }

        public class JobCategoryRequest
        {
            public string? flag { get; set; }
            public string? JOB_Category { get; set; }
        }

        public class JobCategoryRowDto
        {
            public int JOB_Category_ID { get; set; }
            public string JOB_Category { get; set; } = string.Empty;

        }

        public class JobCategoryDto
        {
            public string flag { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public List<JobCategoryRowDto> Rows { get; set; } = new();
        }

        public class JobSubCategoryRowDto
        {
            public int JOB_SUB_Category_ID { get; set; }
            public int Company_Id { get; set; }
            public int JOB_Category_ID { get; set; }
            public string JOB_SUB_Category { get; set; } = string.Empty;

        }

        public class JobSubCategoryDto
        {
            public string flag { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public List<JobSubCategoryRowDto> Rows { get; set; } = new();
        }

        public class PermHireMasterRequest
        {
            public int companyId { get; set; }
            public string? status { get; set; }
            public string? mode { get; set; }
        }

        public class PermHireMasterRowDto
        {
            public int Id { get; set; }
            public string? Company_Id { get; set; }
            public string? LOCATION { get; set; }
            public string? REQ_ID { get; set; }
            public string? REF_ID { get; set; }
            public string? CAND_ID { get; set; }
            public string? CAND_NAME { get; set; }
            public string? DESIGNATION { get; set; }
            public string? DOJ { get; set; }
            public string? VERTICAL { get; set; }
            public string? VH { get; set; }
            public string? CTC { get; set; }
            public string? BILLABLE_CTC { get; set; }
            public string? CONTRACT_TYPE { get; set; }
            public string? BRANCH_CODE { get; set; }
            public string? INVOICE_NO { get; set; }
            public string? INVOICE_STATE { get; set; }
            public string? TOT_INVOICE_AMOUNT { get; set; }
            public string? GST_NO { get; set; }
            public string? STATUS { get; set; }
            public string? CITY { get; set; }
            public string? DOB { get; set; }
            public string? GENDER { get; set; }
            public string? PAN { get; set; }
            public string? AADHAR { get; set; }
            public string? MOBILE_NUMBER { get; set; }
            public string? STATE { get; set; }
            public string? JOB_CATEGORY { get; set; }
            public string? JOB_SUB_CATEGORY { get; set; }
            public string? PLACEMENT_TYPE { get; set; }
            public string? YEAR_EXPERIENCE { get; set; }
            public string? JOINING_DAYS { get; set; }
            public string? Approved_Rejected_ByName { get; set; }
            public string? Approved_Rejected_Remarks { get; set; }
            public string? Approval_Status { get; set; }
            public string? JOBCode { get; set; }
            public string? Consultant { get; set; }
            public string? Offer_Approval_Raised_by_Recruiter { get; set; }
            public string? Requested_on { get; set; }
            public string? Recruiters_Employee_ID { get; set; }
            public string? EntityID { get; set; }
            public string? Organisation_Head { get; set; }
            public string? UPDATED_DATE { get; set; }
            public string? Input_Number { get; set; }
            public string? IS_PO_Applicable { get; set; }
            public string? PO_NUMBER { get; set; }

        }

        public class PermHireMasterDto
        {
            public string CreatedBy { get; set; } = string.Empty;
            public List<PermHireMasterRowDto> Rows { get; set; } = new();
        }
    }
}
