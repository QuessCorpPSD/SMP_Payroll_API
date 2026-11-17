using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface ICostCenterMappingRepository
    {
        Task<List<CostCenterMapping>> GetAllCostCentertDetails(string? costCenter, string schema);
        Task<CostCenterResponse> SaveUpdateDeleteCostCenter([FromBody] CostCenterRequest request, string schema);
        Task<string> PostCostCenterUpload(string xmlString, string userId, string schema);
        DataSet CostCenterExport(string? CostCenterMapName, string schema);
    }
}
