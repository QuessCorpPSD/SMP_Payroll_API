using Microsoft.AspNetCore.Http;
using System.Data;

namespace QPay.BAL.IRepository.BankInvoice
{
    public interface IGenericUpload
    {
        Task<DataSet> GetUploadTypes(int createdBy);
        Task<DataSet> UploadFile(IFormFile? file, int createdBy, string? uploadType);
        DataTable GetExcelColumnNames(string uploadType);
    }
}