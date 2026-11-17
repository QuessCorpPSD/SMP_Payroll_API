using QPay.UI.Invoice;

namespace QPay.API.Models
{
    public class InvoiceInitiateRequestModel
    {
        public List<InitiationRequestUI> invoiceInitiations { get; set; }
        public int TaxTypeId { get; set; }
        public int payPeriod_Id { get; set; }
        public int CreatedBy { get; set; }
    }
}
