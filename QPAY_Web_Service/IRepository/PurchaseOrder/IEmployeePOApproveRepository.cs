using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.PurchaseOrder.EmpPO;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;



namespace QPay.BAL.IRepository.PurchaseOrder
{
    public interface IEmployeePOApproveRepository
    {
        DataSet EmployeePOApproveSearch(string companyId, string siteId, string poNumber, string pricingType,
             string employeeId, string status);
        PoResponse EmployeePOApproveReject(List<EmployeePOApproveReject> employeePOApproveReject, string Createdby);
        DataSet EmployeePOExporttoexcel(string companyId, string siteId, string poNumber, string pricingType,
          string employeeId, string status);
        PoEmpDurationUI GetPoEmpDuration(EmployeePODurationRequest request);
        PoEmpCalculationUI GetPoEmpCalculation(EmployeePOCalculationRequest request);
        PoEmpValueUI GetPoEmpValue(EmployeePOValueRequest request);
    }
}
