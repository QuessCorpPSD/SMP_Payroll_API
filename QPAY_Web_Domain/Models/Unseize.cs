using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay_Web_Domain.Models
{
    public class Unseize
    {
       public string employeeID { get; set;} = "";
        public string axpertEmployeeId { get; set;} = "";
        public string firstName { get; set;} = "";
        public string dateOfJoining { get; set; } = "";
        public string resignationDate { get; set; } = "";
    }

    public class Attachment
    {
        public int? Timesheet_Document_ID { get; set; }
        public string EmployeeID { get; set; } = "";
        public string FileName { get; set; } = "";
        public string DocumentPath { get; set; } = "";
    }
}
