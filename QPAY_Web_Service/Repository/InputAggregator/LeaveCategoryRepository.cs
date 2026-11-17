using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.InputAggregator;
using QPay.DAL.Repository;
using QPay.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Aggregator.LeaveCategory;

namespace QPay.BAL.Repository.InputAggregator
{
    public class LeaveCategoryRepository : ILeaveCategoryRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public LeaveCategoryRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetLeaveCategory(int CompanyId, int siteid, string Flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@Site_Id"] = siteid,
                ["@Action"] = Flag

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Get_LeaveCategory_Master_SiteWise", parameters, 1500);
        }

        public async Task<LeaveCategoryResponse> SaveUpdateDeleteLeaveCategory([FromBody] LeaveCategoryRequest request)
        {
            LeaveCategoryResponse leavecategoryDetails = new LeaveCategoryResponse();

            var parameters = new Dictionary<string, object?>();

            if (request.Mode == "Add")
            {
                parameters = new Dictionary<string, object?>
                {
                    ["@Company_Id"] = request.Company_Id,
                    ["@Site_Id"] = request.Site_Id,
                    ["@Leave_Treate_id"] = request.Leave_Treate_id,
                    ["@Leave_Id"] = request.Leave_Id,
                    ["@Leave_Category_Master_Name"] = request.Leave_Category_Master_Name,
                    ["@IsActive"] = request.IsActive,
                    ["@Created_By"] = request.Created_By,
                    ["@Mode"] = request.Mode
                };
            }

            if (request.Mode == "Edit")
            {
                parameters = new Dictionary<string, object?>
                {
                    ["@Leave_Category_Master_ID"] = request.Leave_Category_Master_ID,
                    ["@Company_Id"] = request.Company_Id,
                    ["@Site_Id"] = request.Site_Id,
                    ["@Leave_Treate_id"] = request.Leave_Treate_id,
                    ["@Leave_Id"] = request.Leave_Id,
                    ["@Leave_Category_Master_Name"] = request.Leave_Category_Master_Name,
                    ["@IsActive"] = request.IsActive,
                    ["@Created_By"] = request.Created_By,
                    ["@Mode"] = request.Mode
                };
            }

            if (request.Mode == "Delete")
            {
                parameters = new Dictionary<string, object?>
                {
                    ["@Leave_Category_Master_ID"] = request.Leave_Category_Master_ID,
                    ["@Company_Id"] = request.Company_Id,
                    ["@Site_Id"] = request.Site_Id,
                    ["@Created_By"] = request.Created_By,
                    ["@Mode"] = request.Mode
                };
            }

            string storeProcedure = "SP_Insert_Update_Delete_LeaveCategory_Master";


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Successfully", StringComparison.OrdinalIgnoreCase))
                    {
                        leavecategoryDetails.response = message;
                    }
                    else
                    {
                        leavecategoryDetails.response = "Failed to import.";
                        leavecategoryDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    leavecategoryDetails.response = "Error while processing response.";
                }
            }
            else
            {
                leavecategoryDetails.response = "Failed";
            }

            return leavecategoryDetails;
        }

    }
}