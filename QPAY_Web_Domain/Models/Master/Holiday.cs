using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.Master
{
    public class Holiday
    {
        public class HolidayMasterResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class HolidayMasterRequest
        {
            public string Created_By { get; set; } = string.Empty;
            public string Mode { get; set; } = string.Empty;
            public List<HolidayMaster> holidaymaster { get; set; }
        }

        public class HolidayMaster
        {
            public string SrNo { get;set; }
            public string CompanyCode { get;set; }
            public string SiteName { get;set; }
            public string State { get;set; }
            public string Fromdate { get;set; }
            public string Todate { get;set; }
            public string HolidayDate { get;set; }
            public string HolidayDescription { get;set; }
            public string HolidayType { get;set; }
        }
    }
}
