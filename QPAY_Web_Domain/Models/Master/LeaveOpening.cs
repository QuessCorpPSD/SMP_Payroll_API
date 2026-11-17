using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.Master
{
    public class LeaveOpening
    {
        public class LeaveOpeningResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class LeaveOpeningTemplateRequest
        {
            public string CompanyId { get; set; }=string.Empty;
            public string SiteId { get; set; } = string.Empty;
            public string CompanyCode { get; set; } = string.Empty;
            public string Fromdate { get; set; } = string.Empty;
            public string Todate { get; set; } = string.Empty;
        }

        public class LeaveOpeningImportRequest
        {
            public string CompanyId { get; set; } = string.Empty;
            public string SiteId { get; set; } = string.Empty;
            public string CompanyCode { get; set; } = string.Empty;
            public string Fromdate { get; set; } = string.Empty;
            public string Todate { get; set; } = string.Empty;
        }
    }
}
