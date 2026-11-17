using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Admin
{
    public class PayPeriodUnlock
    {
        public class UnlockRequest
        {
            public int Allow_Reprocess_Id { get; set; }
            public string? ModifiedBy { get; set; }
        }
    }
}
