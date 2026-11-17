using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Invoice.InvoiceInitiationAgainstProfoma;

namespace QPay.BAL.Repository.Invoice
{
    public class InvoiceInitiationAgainstProfomaRepository : IInvoiceInitiationAgainstProfomaRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public InvoiceInitiationAgainstProfomaRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<InvoiceInitiationAgainstProfomaResponse> ProfomaImport(IFormFile file, int createdBy)
        {
            InvoiceInitiationAgainstProfomaResponse poDetails = new InvoiceInitiationAgainstProfomaResponse();

            if (file == null || file.Length == 0)
            {
                poDetails.response = "File not found";
                return poDetails;
            }

            var uploadsFolder = Path.Combine(
                _configuration["ClaimDocPath"]!.ToString(),
                "Profoma"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var originalFileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(originalFileName);

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{Guid.NewGuid():N}{extension}";

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save uploaded file
            await using (var stream = new FileStream(
                filePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None))
            {
                await file.CopyToAsync(stream);
            }

            try
            {

                DataSet ds = ExcelToDataSet(filePath);

                if (ds.Tables.Count == 0)
                {
                    poDetails.response =
                        "Excel sheet is empty or not formatted correctly.";

                    return poDetails;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    poDetails.response =
                        "Excel sheet is empty or not formatted correctly.";

                    return poDetails;
                }


                using var xmlWriter = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);

                string xmlInput = xmlWriter.ToString().Replace("ProfomaImport", "Table");

                string storeProcedure =
                    @"Proc_ProfomaImport";

                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", createdBy);

                var res = await _dbRepository.GetItemsAsync(
                    storeProcedure,
                    parameters
                );

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList =
                            JsonConvert.DeserializeObject<List<InvoiceInitiationAgainstProfomaResponseModel>>(res);

                        var message =
                            resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains(
                                "Import Successfully Done",
                                StringComparison.OrdinalIgnoreCase))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";

                            poDetails.errors = res
                                .Split(
                                    new[] { '\r', '\n' },
                                    StringSplitOptions.RemoveEmptyEntries
                                )
                                .ToList();
                        }
                    }
                    catch
                    {
                        poDetails.response =
                            "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            finally
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Unable to delete file: {ex.Message}"
                    );
                }
            }

            return poDetails;
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }


        public async Task<DataSet> GetProfoma(int CompanyId, int PayperiodId, string flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@PayPeriodId"] = PayperiodId,
                ["@Action"] = flag
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageGstInvoiceInitiateAgainstProfoma", parameters, 1500);
        }

        public async Task<InvoiceInitiationAgainstProfomaResponse> Initiate([FromBody] InitiateRequestDto request)
        {
            InvoiceInitiationAgainstProfomaResponse initationDetails = new InvoiceInitiationAgainstProfomaResponse();

            if (request == null || request.Rows == null || !request.Rows.Any())
            {
                initationDetails.response = "Invalid Initiate request.";
            }

            var xmlInput = BuildInitiateXml(request);

            string storeProcedure = "Proc_ManageGstInvoiceInitiateAgainstProfoma";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@CompanyId", request.CompanyId);
            parameters.Add("@PayPeriodId", request.PayPeriodId);
            parameters.Add("@Action", request.flag);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<InvoiceInitiationAgainstProfomaResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data saved successfully"))
                    {
                        initationDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data submitted successfully."))
                    {
                        initationDetails.response = message;
                    }
                    else
                    {
                        initationDetails.response = "Failed to Save.";
                        initationDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    initationDetails.response = "Error while processing response.";
                }
            }
            else
            {
                initationDetails.response = "Failed";
            }


            return initationDetails;
        }

        private string BuildInitiateXml(InitiateRequestDto request)
        {
            var sb = new StringBuilder();
            sb.Append("<InitiateDetails>");

            foreach (var row in request.Rows)
            {
                sb.Append("<Initiate>");
                sb.AppendFormat("<RowNumber>{0}</RowNumber>", row.RowNumber);
                sb.AppendFormat("<CompanyId>{0}</CompanyId>", row.CompanyId);
                sb.AppendFormat("<PayPeriodId>{0}</PayPeriodId>", row.PayPeriodId);
                sb.AppendFormat("<CostCenterMappingId>{0}</CostCenterMappingId>", row.CostCenterMappingId);
                sb.AppendFormat("<StateId>{0}</StateId>", row.StateId);
                sb.AppendFormat("<GroupDetailId>{0}</GroupDetailId>", row.GroupDetailId);
                sb.AppendFormat("<InputNumber>{0}</InputNumber>", row.InputNumber);
                sb.AppendFormat("<EmployeeCount>{0}</EmployeeCount>", row.EmployeeCount);
                sb.AppendFormat("<NetPay>{0}</NetPay>", row.NetPay);
                sb.AppendFormat("<CTC>{0}</CTC>", row.CTC);
                sb.AppendFormat("<ServiceChargeAmount>{0}</ServiceChargeAmount>", row.ServiceChargeAmount);
                sb.Append("</Initiate>");
            }

            sb.Append("</InitiateDetails>");
            return sb.ToString();
        }
    }
}
