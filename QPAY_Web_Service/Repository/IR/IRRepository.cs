using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.IR;
using QPay.DAL.Repository;

namespace QPay.BAL.Repository.IR
{
    public class IRRepository : IIRRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public IRRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetLastThreeYear()
        {
            var parameters = new Dictionary<string, object?>
            {
               
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Get_Last_Three_Year", parameters, 1500);
        }

        public async Task<DataSet> GetIREmployeeDetail(string? Company_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_GET_IR_Employee_Details", parameters, 1500);
        }

        public async Task<DataSet> GetIRDetail(string? Employee_Id, string? Month, string? Year)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Employee_Id"] = Employee_Id,
                ["@Month"] = Month,
                ["@Year"] = Year
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_GET_IR_Details", parameters, 1500);
        }

        public async Task<DataSet> GetIR8ADetail(string? Employee_Id,string? Year)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Employee_Id"] = Employee_Id,
                ["@Year"] = Year
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_GET_IR_8A_Details", parameters, 1500);
        }

    }
}
