using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IInvoiceLegalEntityRepository
    {
        Task<DataSet> Search(string schema);
        Task<DataSet> Create(QuessLegalEntityRequest request, string schema);

    }
}
