using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class EmployeeDetailPO
    {
        public List<EmployeePO> employees {  get; set; }
        public List<EmployeePODetail> EmployeePODetails { get; set; }
    }
    public class EmployeePO
    {
        public int? PO_ID { get; set; }
        public string PoNumber { get; set; } = "";
        public string StartDate { get; set; } = ""; 
        public string EndDate { get; set; } = "";

        [Column("PO TOTAL VALUE")]
        public double? POTOTALVALUE { get; set; }

        [Column("PO Balance")]
        public double? POBalance { get; set; }

        [Column("UTILIZED AMOUNT")]
        public double? UTILIZEDAMOUNT { get; set; }
    }

    public class EmployeePODetail
    {
        public int? EMployeeListID { get; set; }
        public int PO_ID { get; set; }
        [Column("COMPANY CODE")]
        public string COMPANYCODE { get; set; } = "";
        [Column("SITE NAME")]
        public string SITENAME { get; set; } = "";

        [Column("EMPLOYEE ID")]
        public int? EMPLOYEEID { get; set; }

        [Column("EMPLOYEE NAME")]
        public string EMPLOYEENAME { get; set; } = "";

        public string Employee_Code { get; set; } = "";
        public string DOJ { get; set; } = "";

        public string FixedRate { get; set; } = "";

        [Column("PO START DATE")]
        public string POSTARTDATE { get; set; }

        [Column("PO END DATE")]
        public string POENDDATE { get; set; }

        public int StatusID {  get; set; }

        public string EmployeePOAttachment { get; set; } = "";
        public string STATUS_NAME { get; set; } = "";
        public string PricingType { get; set; } = "";

        public string IsActive { get; set; } = "";
        public string PoNumber { get; set; } = "";
        public string ItemType { get; set; } = "";
        public int? Noclick {  get; set; } = 0;

    }
}
