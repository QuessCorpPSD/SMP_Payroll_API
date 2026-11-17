using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class RAD_SP_INV_SHOWLIST_BYGROUPUI
    {
        public string GROUP_NAME { get; set; } = "";

        public string CODE { get; set; } = "";

        public string? DESCR { get; set; } = "";
        public string? NOTES { get; set; } = "";

        public int ROWID { get; set; }
    }
}
