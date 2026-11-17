using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Aggregator
{
    public class LeaveCategory
    {
        public class LeaveCategoryResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class LeaveCategoryRequest
        {
            public string Created_By { get; set; } = string.Empty;
            public string Mode { get; set; } = string.Empty;
            public int? Leave_Category_Master_ID { get; set; }
            public int Company_Id { get; set; }
            public int Site_Id { get; set; }
            public int? Leave_Treate_id { get; set; }
            public int? Leave_Id { get; set; }
            public string Leave_Category_Master_Name { get; set; } = string.Empty;
            public int? IsActive { get; set; } 
        }
    }
}
