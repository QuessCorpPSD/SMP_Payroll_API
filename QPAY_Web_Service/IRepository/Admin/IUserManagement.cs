using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Admin
{
    public interface IUserManagement
    {
        Task<DataSet> GetRoleTypes(string schema);
        Task<DataSet> GetReportingTo(string schema);

        Task<DataSet> GetAccessType(string schema);
        Task<DataSet> Search(int? UserId, String? UserName, int? RoleId, int? IsCheck, string schema);

        Task<DataSet> Create(string xml, int createdBy, string mode, string UserDetails, string schema);

        Task<DataSet> Unlock(string xml, int createdBy, string mode, string UserDetails, string schema);

    }
}
