using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.BankInvoice
{
    public class GenericUploadModel
    {
        public IFormFile? File { get; set; }
        public int CreatedBy { get; set; }
        public string? UploadType { get; set; }
    }
}
