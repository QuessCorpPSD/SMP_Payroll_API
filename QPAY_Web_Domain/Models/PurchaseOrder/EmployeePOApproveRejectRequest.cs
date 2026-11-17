namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class EmpPO
    {
        public class EmployeePOApproveRejectRequest
        {
            public List<EmployeePOApproveReject> employeePOApproveReject { get; set; }
            public string CreatedBy { get; set; } = "";
        }

        public class EmployeePOApproveReject
        {
            public string PO_ID { get; set; } = "";
            public string EMP_LIST_ID { get; set; } = "";
            public string CLIENT_EMP_ID { get; set; } = "";
            public string STATUS { get; set; } = "";
            public string REMARKS { get; set; } = "";

        }

        public class EmployeePODurationRequest
        {
            public string PO_ID { get; set; } = "";
            public string Startdate { get; set; } = "";
            public string ENDDATE { get; set; } = "";
        }

        public class EmployeePOCalculationRequest
        {
            public string PO_ID { get; set; } = "";
            public string CLIENTEMPID { get; set; } = "";
            public string PRICINGTYPE { get; set; } = "";
            public string Startdate { get; set; } = "";
            public string ENDDATE { get; set; } = "";
            public string QUANTITYTYPE { get; set; } = "";
        }

        public class EmployeePOValueRequest
        {
            public string QUANTITYTYPE { get; set; } = "";
            public string QUANTITY { get; set; } = "";
            public string FIXEDRATE { get; set; } = "";
        }
        public class PoEmpDurationUI
        {
            public string DURATION { get; set; } = "";
            public string Result { get; set; } = "";
        }

        public class PoEmpCalculationUI
        {
            public string POQUANTITY { get; set; } = "";
        }

        public class PoEmpValueUI
        {
            public string UNITPRICE { get; set; } = "";
            public string TOTAL { get; set; } = "";
            public string MONTHLYRATE { get; set; } = "";
        }
    }
}
