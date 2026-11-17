using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.SalaryAdvance
{
    public class Request
    {

        public int Salary_Advance_id { get; set; }
        public int Serial_No { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int Pay_Period_Id { get; set; }
        public string Pay_Period { get; set; }
        public int Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Code { get; set; }
        public string PayCode { get; set; }
        public int PayCode_id { get; set; }
        public string RequestType { get; set; }
        public int RequestType_Id { get; set; }
        public string SalaryAdvanceStatus { get; set; }
        public string Amount { get; set; }

        public string ApproveAmount { get; set; }
        public int Noofinstallment { get; set; }
        public string Error_Message { get; set; }

    }

    public class RequestResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

}
