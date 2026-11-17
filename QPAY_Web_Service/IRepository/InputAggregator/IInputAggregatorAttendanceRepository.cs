using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Aggregator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.InputAggregator
{
    public interface IInputAggregatorAttendanceRepository
    {
        Task<DataSet> QuessLeaveMaster(string schema);
        Task<DataSet> leaveTypeMaster(string schema);
        Task<DataSet> Createleavemapping(AttendanceAggregatorRequest request, string schema);

        Task<DataSet> Createleavetype(leaveTypeMasterRequest request, string schema);

        Task<DataSet> SearchLeaveTypeMapping(int? companyId, int? siteid, string? Flag, string schema);

        Task<DataSet> LeavePolicyMaster(string schema);

        Task<DataSet> QuessAttendanceAttributeMaster(string schema);
        Task<DataSet> ClientAttendanceAttribute(int? companyId, int? siteid, string schema);

        Task<DataSet> ClientAttendanceAttributeMaster(int? companyId, int? siteid, string schema);

        Task<DataSet> QuessAttendanceAttributeClient(int? companyId, string schema);

        Task<DataSet> Search(int? companyId, int? siteid, string schema);

        Task<RequestResponse> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy, string schema);

        Task<RequestResponse> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy, string schema);

        Task<DataSet> AttributesMappingCreate(AttributesMappingRequest items, string schema);

        Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId, string schema);

        Task<DataSet> billableReport(int? companyId, int? payPeriodId, int? siteid, string schema);

        Task<DataSet> AttendanceReport(int? companyId, int? payPeriodId, int? siteid, string schema);

        Task<DataSet> ClientAttributesDelete(int? companyId, int? siteid, int? templateId, string CreatedBy, string schema);

        Task<DataSet> FinalSubmission(int? companyId, int? payPeriodId, string schema);

    }
}
