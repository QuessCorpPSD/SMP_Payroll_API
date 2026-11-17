namespace QPay_Web_Domain.Models
{
    public class Timesheet
    {
        public class TimesheetResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class TimesheetAttachment
        {
            public string Timesheet_Document_ID { get; set; } = string.Empty;
            public string EmployeeID { get; set; } = string.Empty;
            public string FileName { get; set; } = string.Empty;
            public string DocumentPath { get; set; } = string.Empty;
        }

        public class DayEntryDto
        {
            public DateTime Date { get; set; }
            public string OT { get; set; } = string.Empty;
            public string HOT { get; set; } = string.Empty;
        }

        public class TimesheetRowDto
        {
            public int SlNo { get; set; }
            public string EmpID { get; set; } = string.Empty;
            public string EmployeeName { get; set; } = string.Empty;
            public string DOJ { get; set; } = string.Empty;
            public string Seperation { get; set; } = string.Empty;
            public string WDWH { get; set; } = string.Empty;
            public string DEHE { get; set; } = string.Empty;
            public int L { get; set; }
            public int H { get; set; }
            public int CO { get; set; }
            public int WO { get; set; }
            public string Status { get; set; } = string.Empty;
            public string Remarks { get; set; } = string.Empty;
            public string Approver { get; set; } = string.Empty;
            public string OT { get; set; } = string.Empty;
            //public string OTWKD { get; set; } = string.Empty;
            //public string OTWND { get; set; } = string.Empty;
            //public string NSOTH { get; set; } = string.Empty;
            //public string POTRS { get; set; } = string.Empty;

            public List<DayEntryDto> DayEntries { get; set; } = new();
        }

        public class TimesheetRequestDto
        {
            public string CompanyCode { get; set; } = string.Empty;
            public string SiteId { get; set; } = string.Empty;
            public int PayPeriodId { get; set; }
            public string PayPeriod { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public List<TimesheetRowDto> Rows { get; set; } = new();
        }



        public class RejectEmpId
        {
            public List<EmployeeItem>? EmpID { get; set; }
            public string companyCode { get; set; } = string.Empty;
            public int payPeriodId { get; set; }
            public int? userId { get; set; }
        }
        public class EmployeeItem
        {
            public string EmpID { get; set; } = "";
        }
    }
}