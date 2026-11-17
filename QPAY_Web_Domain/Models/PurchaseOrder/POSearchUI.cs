using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.PurchaseOrder
{

    public class POSummaryUI
    {
        public List<POSearchUI> pOSearches { get; set; }
        public CompanyPOSummaryUI summary { get; set; }
    }
    public class POSearchUI
    {
        public string POID { get; set; } = string.Empty;
        public string PO_ID { get; set; } = string.Empty;
        public string SLNO { get; set; } = string.Empty;

        public string CompanyCode { get; set; } = string.Empty;

        public string PONumber { get; set; } = string.Empty;

        public string PricingTYPE { get; set; } = string.Empty;

        public string POValue { get; set; } = string.Empty;
        public string POStartDate { get; set; } = string.Empty;
        public string POEndDate { get; set; } = string.Empty;
        public string POType { get; set; } = string.Empty;
        public string PricingType { get; set; } = string.Empty;

        public string DocPath { get; set; } = string.Empty;
        public string Createddate { get; set; } = string.Empty;

        public string PricingType_Defauly { get; set; } = string.Empty;
        public string Company_ID { get; set; } = string.Empty;

        public string PO_CategoryID { get; set; } = string.Empty;
        public string DELETE { get; set; } = string.Empty;
    }


    public class CompanyPOSummaryUI
    {
        public double POTOTAL { get; set; }
        public double POBalance { get; set; }
        public double UTILIZEDAMOUNT { get; set; }
        public double TOTALTAGGEDVALUE { get; set; }
    }

    public class POUI
    {
        public int POID { get; set; }
        public string PONUMBER { get; set; } = string.Empty;
    }

    public class POStatus
    {
        public int POSID { get; set; }
        public int STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
        public int ISACTIVE { get; set; }
    }
}
