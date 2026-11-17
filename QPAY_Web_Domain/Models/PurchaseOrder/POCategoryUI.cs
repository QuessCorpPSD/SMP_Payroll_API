using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class POCategoryUI
    {
        public int? CategoryID {  get; set; }
        public string Category_Name { get; set; } = string.Empty;
    }
}
