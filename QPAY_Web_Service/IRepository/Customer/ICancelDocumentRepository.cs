using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface ICancelDocumentRepository
    {
        Task<List<CancelDocument>> Search(int companyId, int payPeriodId);
        Task<string> UploadDocument(string xml, int userId);
    }
}
