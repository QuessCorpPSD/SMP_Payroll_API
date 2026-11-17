using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.Master
{
    public class LeaveRule
    {
        public class LeaveRuleResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class LeaveTypes
        {
            public string Leavetype { get; set; }
        }

        public class LeaveRuleRequest
        {
            public string Created_By { get; set; } = string.Empty;
            public string Mode { get; set; } = string.Empty;
            public List<LeaveRuleMaster> leaverulemaster { get; set; }
        }

        public class LeaveRuleMaster
        {
            public string SrNo { get; set; } = string.Empty;
            public string Company_ID { get; set; } = string.Empty;
            public string Company_Name { get; set; } = string.Empty;
            public string State { get; set; } = string.Empty;
            public string Group_name { get; set; } = string.Empty;
            public string GroupDetail_Id { get; set; } = string.Empty;
            public string EffectiveDate { get; set; } = string.Empty;
            public string Start_Month { get; set; } = string.Empty;
            public string End_Month { get; set; } = string.Empty;
            public string leavetype { get; set; } = string.Empty;
            public string Invoice_leavedetails { get; set; } = string.Empty;
            public string Leavedetails { get; set; } = string.Empty;
            public string Carryforward { get; set; } = string.Empty;
            public string MaxDays { get; set; } = string.Empty;
            public string Encashment { get; set; } = string.Empty;
        }

    }
}
