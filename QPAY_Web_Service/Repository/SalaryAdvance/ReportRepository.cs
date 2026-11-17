using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.BAL.IRepository.SalaryAdvance;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.SalaryAdvance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.IRepository.Repository.PurchaseOrder.PurchaseOrderRepository;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace QPay.BAL.Repository.SalaryAdvance
{
    public class ReportRepository : IReportRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? CompanyId, int? PayPeriod_Id)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PayPeriod"] = PayPeriod_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ExportSalarydvanceReport", parameters);
        }

    }
}
