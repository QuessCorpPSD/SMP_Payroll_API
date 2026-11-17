using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Admin
{
    public interface ICompanyPermissionRepository
    {
        Task<DataSet> GetUsername(string schema);
        Task<DataSet> Search(int? UserId, int? Businessunitnameid, int? CompanyPermissionId, string schema);
        Task<DataSet> ExportToExcel(int? UserId, int? Businessunitnameid, string schema);
        Task<DataSet> LoadCompany(int? BusinessUnitNameId, int? BusinessZonenName, string schema);
        //Task<DataSet> CreateCompanyPermission(CompanyPermission request);

        Task<DataSet> CreateUpdateDelete(string xml, int createdBy, string mode, string schema);

        Task<DataSet> Editdetails(int? Userid, int? Businessunitnameid, int? CompanyPermissionId, string schema);

    }

}
