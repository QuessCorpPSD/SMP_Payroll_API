using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class BulkPOUI
    {
        public string Company_Code {  get; set; }
        public string PoNumber { get; set; }
        public string POValue { get; set; }
        public string PODate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Pricing_Type { get; set; }
        public string CurrencyType { get; set; }
        public string Billing_Type { get; set; }

        public string InternalExternal { get; set; }

        public string POQuantuty_Type { get; set; }


        public string ExtentionEndDate { get; set; }
        public string ExtentionPOValue { get; set; }
        public string Mode { get; set; }
        public string PO_Category { get; set; }
    }

    public class BulkUploadResponse
    {
        public string Error_Message { get; set; } = "";
    }
}
