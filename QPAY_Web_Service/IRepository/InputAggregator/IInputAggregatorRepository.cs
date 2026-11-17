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
    public interface IInputAggregatorRepository
    {
        Task<DataSet> QuessAttributeMaster();
        Task<DataSet> ClientAttributes(int? companyId, int? siteid);
        Task<DataSet> Search(int? companyId, int? siteid);
        Task<DataSet> billableReport(int? companyId, int? payPeriodId);

        Task<RequestResponse> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId);

        Task<RequestResponse> MISCUpload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId);

        Task<RequestResponse> OTRateUpload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId);

        Task<DataSet> CompanyPaycode(int? companyId);
        Task<DataSet> MISCPaycodeSearch(int? companyId, int? siteid, string? Flag);

        Task<DataSet> MISCPaycodes(int? companyId, int siteid);

        Task<DataSet> MISCPaycodeMapping(MISCPaycodeMappingRequest request);

        Task<DataSet> FinalSubmission(int? companyId, int? payPeriodId);

        Task<DataSet> ClientAttributesDelete(int? companyId, int? siteid, int? templateId,string CreatedBy);
        Task<DataSet> ClientAttributesMaster(int? companyId, int? siteid);

        Task<DataSet> MappingAttributesDelete(int? companyId, int? templateId, int? Quess_Template_Field_Id, string CreatedBy);

        Task<RequestResponse> ClientAttributesUploadOI(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> AttributesMappingUploadOI(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> UploadOI(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId);

    }
}
