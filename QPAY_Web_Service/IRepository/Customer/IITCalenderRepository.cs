using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IITCalenderRepository
    {       
        Task<DataSet> Search(int? companyId, int? financialYearId, string schema);
        Task<DataSet> GetFinancialYear(string schema);
        Task<DataSet> Create(ITCalenderRequest request, string schema);
    }
}
