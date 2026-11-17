using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.Master
{
    public class VendorEmployee
    {
        public class VendorEmployeeResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class VendorEmployeeRequest
        {
            public string EMPLOYEE_CODE { get; set; }=string.Empty;
            public string EMPLOYEE_NAME { get; set; } = string.Empty;
            public string CLIENT_NAME { get; set; } = string.Empty;
            public string COMPANY_CODE { get; set; } = string.Empty;
            public string UNIQUE_TYPE { get; set; } = string.Empty;
            public string EACTIVE { get; set; } = string.Empty;
            public string DATE_JOINED { get; set; } = string.Empty;
            public string MAP_NAME { get; set; } = string.Empty;
            public string VENDOR { get; set; } = string.Empty;
            public string DOS { get; set; } = string.Empty;
            public string VERTICAL { get; set; } = string.Empty;
            public string PO_VALUE { get; set; } = string.Empty;
            public string PO_START_DATE { get; set; } = string.Empty;
            public string PO_END_DATE { get; set; } = string.Empty;
            public string REQUISITIONER_NAME { get; set; } = string.Empty;
            public string SALARY { get; set; } = string.Empty;
            public string MOBILE_NUMBER { get; set; } = string.Empty;
            public string PAN_NUMBER { get; set; } = string.Empty;
            public string EMAIL_ID { get; set; } = string.Empty;
            public string FULL_ADDRESS { get; set; } = string.Empty;
            public string STATE_NAME { get; set; } = string.Empty;
            public string GROUP_NAME { get; set; } = string.Empty;
        }

    }
}
