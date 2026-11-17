using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.Master
{
    public class LeaveMaster
    {
        public int? Leavemasterid { get; set; }
        public string Company_Code { get; set; } = "";
        public string SiteName { get; set; } = "";
        public string LeaveType { get; set; } = "";
    }

    public class LeavetypeDD
    {
        public int? Leavevariantsid { get; set; }
        public string Leavetype { get; set; } = "";
    }

    public class LeaveMasterAdd
    {
        public int? CompanyId { get; set; }
        public string CompanyCode { get; set; } = "";
        public string SiteId { get; set; } = "";
        public string SiteName { get; set; } = "";
        public string Leavetype { get; set; } = "";
        public string UserId { get; set; } = "";
    }
}
