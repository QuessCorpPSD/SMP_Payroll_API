using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Admin.PayPeriodUnlock;
using static QPay.UI.Models.GlobalMaster.LWFClass;

namespace QPay.BAL.IRepository.Admin
{
    public interface IPayPeriodUnlockRepository
    {
        Task<DataSet> Search();
        Task<DataSet> Unlock(UnlockRequest request);
    }
}
