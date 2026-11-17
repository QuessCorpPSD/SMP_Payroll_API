namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class POBulkModelRequest
    {
        public string? ActionType { get; set; } = string.Empty;
        public int? EmployeeId { get; set; } 
        public Files[]? File { get; set; }


    }
    public class Files
    {
        public string? name { get; set; } = string.Empty;
        public string? type { get; set; } = string.Empty;
        public int? size {  get; set; } 
        public string? content { get; set; } = string.Empty;
    }
}
