using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Aggregator.LeaveCategory;


namespace QPay.BAL.IRepository.InputAggregator
{
    public interface ILeaveCategoryRepository
    {
        Task<DataSet> GetLeaveCategory(int CompanyId, int siteid, string Flag);
        Task<LeaveCategoryResponse> SaveUpdateDeleteLeaveCategory([FromBody] LeaveCategoryRequest request);
    }
}
