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
    public interface IPayFrequencyRepository
    {       
        Task<DataSet> Search(int? companyId, string schema);
        Task<DataSet> CopySearch(int companyId, string startDate, string endDate, string schema);
        Task<DataSet> ExportToExcel(int? companyId, string schema);
        Task<DataSet> GetGroupName(int? companyId, string schema);
        Task<DataSet> GetData( string startDate, string endDate, string schema);
        Task<DataSet> CheckPayFrequencyExists(int companyId, string startDate, string endDate, string payPeriod, string schema);

        Task<DataSet> Create(PayFrequencyRequest request, string schema);
    }
}
