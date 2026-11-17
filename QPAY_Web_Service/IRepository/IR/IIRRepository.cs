using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.IR
{
    public interface IIRRepository
    {
        Task<DataSet> GetLastThreeYear();
        Task<DataSet> GetIREmployeeDetail(string? Company_Id);
        Task<DataSet> GetIRDetail(string? Employee_Id, string? Month, string? Year);
        Task<DataSet> GetIR8ADetail(string? Employee_Id, string? Year);
    }
}
