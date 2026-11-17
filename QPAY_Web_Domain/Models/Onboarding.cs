using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class Onboarding
    {
        public string? offerId { get; set; }
        public string? employeeName { get; set; }
        public string? fatherName { get; set; }
        public string? gender { get; set; }
        public string? doj { get; set; }
        public string? dob { get; set; }
        public string? designation { get; set; }
        public string? jobState { get; set; }
        public string? jobLocation { get; set; }
    }

    public class NewJoineeModel
    {
        public int companyId { get; set; }
        public int payPeriodId { get; set; }
    }
    public class FinalSubmission
    {
        public string? InputName { get; set; }
        public string? HeadCount { get; set; }
        public string? CurrentStatus { get; set; }
        public string? InputLotNumber { get; set; }
        public string? FirstName { get; set; }
        public bool? IsProcessed { get; set; }
        public int? Revised { get; set; }
        public bool? IsSubmitted { get; set; }
        public bool? IsEmployeeID { get; set; }

    }

    public class FileJson{
        public string? FileName { get; set; }
        }
}
