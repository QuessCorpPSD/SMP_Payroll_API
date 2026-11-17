using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay_Web_Domain.Models.Timesheet;

namespace QPay.UI.Invoice
{
    public class InvoiceInitiationAgainstProfoma
    {
        public class InvoiceInitiationAgainstProfomaResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class InvoiceInitiationAgainstProfomaResponseModel
        {
            public string? Result { get; set; }
            public string? Error_Message { get; set; }
            public string? ErrorMessage { get; set; }
        }


        public class InitiateRowDto
        {
            public int RowNumber { get; set; }
            public int CompanyId { get; set; }
            public int PayPeriodId { get; set; }
            public int CostCenterMappingId { get; set; }
            public int StateId { get; set; }
            public int GroupDetailId { get; set; }
            public int InputNumber { get; set; }
            public int EmployeeCount { get; set; }
            public string NetPay { get; set; } = string.Empty;
            public string CTC { get; set; } = string.Empty;
            public string ServiceChargeAmount { get; set; } = string.Empty;

        }

        public class InitiateRequestDto
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string flag { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public List<InitiateRowDto> Rows { get; set; } = new();
        }
    }
}
