using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using QPay.UI_Domain.Models.Master;
using System.Data;

namespace QPay.IRepository.Repository.Master
{
    public class InvoiceRuleRepository : IInvoiceRuleRepository
    {
        private readonly DbRepository _dbRepository;

        public InvoiceRuleRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<InvoiceRule>> GetAllInvoiceRule(int? companyId, string? siteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            parameters.Add("@Site_id", siteId);

            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllInvoicingRule", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InvoiceRule>>(res) ?? new List<InvoiceRule>();
            }

            return new List<InvoiceRule>();
        }
        public async Task<string> PostAddInvoiceRule(InvoiceRuleAdd invoiceRuleAdd)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", invoiceRuleAdd.CompanyId);
            parameters.Add("@SITE_Id", invoiceRuleAdd.SiteId);
            parameters.Add("@CompanyCode", invoiceRuleAdd.CompanyCode);
            parameters.Add("@SITE_NAME", invoiceRuleAdd.SiteName);
            parameters.Add("@DAYS_PER_MONTH", invoiceRuleAdd.DaysPerMonth ?? (object)DBNull.Value, DbType.Int32);
            parameters.Add("@BillingType", invoiceRuleAdd.BillingType);
            parameters.Add("@WEEKENDS", invoiceRuleAdd.WeekendsRule);
            parameters.Add("@HOLIDAYS", invoiceRuleAdd.HolidaysRule);
            parameters.Add("@COMP_OFF", invoiceRuleAdd.ComppoffRule);
            parameters.Add("@MATERNITY", invoiceRuleAdd.MaternityLeave);
            parameters.Add("@LEAVE_ADDITION", invoiceRuleAdd.LeaveCredit);
            parameters.Add("@LEAVE_RULE", invoiceRuleAdd.LeaveRule);
            parameters.Add("@OT", invoiceRuleAdd.OtRule);
            parameters.Add("@Gratuity", invoiceRuleAdd.Gratuity);
            parameters.Add("@Rebates", invoiceRuleAdd.Rebates);
            parameters.Add("@ServiceFee", invoiceRuleAdd.ServiceFeeOnExpenses);
            parameters.Add("@Reimbursement", invoiceRuleAdd.Reimbursement);
            parameters.Add("@Discounts", invoiceRuleAdd.Discounts);
            parameters.Add("@BILLABLE_DAYS_Formula", invoiceRuleAdd.BillableDaysFormula);
            parameters.Add("@AsPerTimesheet",
           invoiceRuleAdd.DaysAsPerTimesheet is bool b
               ? (object)(b ? "1" : "0")
               : DBNull.Value);
            parameters.Add("@Created_by", invoiceRuleAdd.UserId);
            parameters.Add("@Leavetypes", invoiceRuleAdd.LeaveTypes);
            parameters.Add("@LeavePeriod", invoiceRuleAdd.PayPeriodFrom + "-" + invoiceRuleAdd.PayPeriodTo);
            parameters.Add("@Carryfarward", invoiceRuleAdd.CarryForward);
            parameters.Add("@NoofCarryfarward", invoiceRuleAdd.NoOfCarryForwards ?? (object)DBNull.Value, DbType.Int32);

            var res = await this._dbRepository.GetItemsAsync("Get_tb_ClientWiseInvoicingRules_Insert_new", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> PostEditInvoiceRule(InvoiceRuleEdit invoiceruleEdit)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@BILLING_TYPE", invoiceruleEdit.BillingType ?? (object)DBNull.Value);
            parameters.Add("@OT", invoiceruleEdit.OtRule ?? (object)DBNull.Value);
            parameters.Add("@Gratuity", invoiceruleEdit.Gratuity ?? (object)DBNull.Value);
            parameters.Add("@Reimbursement", invoiceruleEdit.Reimbursement ?? (object)DBNull.Value);
            parameters.Add("@ServiceFee", invoiceruleEdit.ServiceFeeOnExpenses ?? (object)DBNull.Value);
            parameters.Add("@Rebates", invoiceruleEdit.Rebates ?? (object)DBNull.Value);
            parameters.Add("@Discounts", invoiceruleEdit.Discounts ?? (object)DBNull.Value);

            parameters.Add("@DAYS_PER_MONTH", invoiceruleEdit.DaysPerMonth ?? (object)DBNull.Value, DbType.Int32);

            parameters.Add("@WEEKENDS", invoiceruleEdit.WeekendsRule ?? (object)DBNull.Value);
            parameters.Add("@HOLIDAYS", invoiceruleEdit.HolidaysRule ?? (object)DBNull.Value);
            parameters.Add("@COMP_OFF", invoiceruleEdit.ComppoffRule ?? (object)DBNull.Value);
            parameters.Add("@MATERNITY", invoiceruleEdit.MaternityLeave ?? (object)DBNull.Value);

            parameters.Add("@LEAVE_ADDITION", invoiceruleEdit.LeaveCredit ?? (object)DBNull.Value);
            parameters.Add("@LEAVE_RULE", invoiceruleEdit.LeaveRule ?? (object)DBNull.Value);
            parameters.Add("@BILLABLE_DAYS_LESS_THAN_ZERO", invoiceruleEdit.BillableDaysFormula ?? (object)DBNull.Value);

            parameters.Add("@Created_by", invoiceruleEdit.UserId ?? (object)DBNull.Value);
            parameters.Add("@Leavetypes", invoiceruleEdit.LeaveTypes ?? (object)DBNull.Value);

            // LeavePeriod is a combined string; allow null if either is missing
            parameters.Add("@LeavePeriod",
                string.IsNullOrEmpty(invoiceruleEdit.PayPeriodFrom) ||
                string.IsNullOrEmpty(invoiceruleEdit.PayPeriodTo)
                    ? (object)DBNull.Value
                    : $"{invoiceruleEdit.PayPeriodFrom}-{invoiceruleEdit.PayPeriodTo}");

            parameters.Add("@Carryfarward", invoiceruleEdit.CarryForward ?? (object)DBNull.Value);
            parameters.Add("@NoofCarryfarward", invoiceruleEdit.NoOfCarryForwards ?? (object)DBNull.Value, DbType.Int32);

            // DaysAsPerTimesheet supports null → convert to "1" / "0" / null
            parameters.Add("@AsPerTimesheet",
                invoiceruleEdit.DaysAsPerTimesheet is bool b
                    ? (object)(b ? "1" : "0")
                    : DBNull.Value);

            parameters.Add("@InvoicingRulesID", invoiceruleEdit.InvoicingRulesID ?? (object)DBNull.Value);


            var res = await this._dbRepository.GetItemsAsync("Get_tb_ClientWiseInvoicingRules_Update_New", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> PostDeleteInvoiceRule(int invoicingRulesID)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@invoicingRulesID", invoicingRulesID);
            var res = await this._dbRepository.GetItemsAsync("Proc_Delete_InvoiceRule", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public DataSet GetInvoiceRuleTemplate(int companyId, string siteName)
        {
            DataSet ds = this._dbRepository.GetInvoiceRuleTemplate(companyId, siteName);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company.");
            }

        }
        public async Task<string> PostInvoiceRuleUpload(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Xmldoc", xmlString);
            parameters.Add("@Created_By", userId);

            var res = await this._dbRepository.GetItemsAsync("proc_bulk_InvoiceRule", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public DataSet InvoiceRuleExport(int companyId, int siteCode)
        {
            DataSet ds = this._dbRepository.InvoiceRuleExport(companyId, siteCode);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given Parameters.");
            }

        }
    }
}
