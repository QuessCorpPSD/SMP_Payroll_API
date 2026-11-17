using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using static QPay.UI_Domain.Models.Master.LeaveRule;


namespace QPay.IRepository.Repository.Master
{
    public class LeaveRuleRepository : ILeaveRuleRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public LeaveRuleRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetLeaveRuleCompanywise(int CompanyId, int SiteId, string State)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@SiteId"] = SiteId,
                ["@StateName"] = State
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_QZONE_LeaveRules_Search_CCSN", parameters, 1500);
        }

        public async Task<DataSet> GetLeaveRuleTemplate()
        {
            var parameters = new Dictionary<string, object?>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Qzone_LeaveRule_DownLoadTemplate", parameters, 1500);
        }

        public async Task<LeaveRuleResponse> UploadLeaveRule(IFormFile file, [FromForm] string User)
        {
            LeaveRuleResponse leaveruleDetails = new LeaveRuleResponse();
            string sFileName = "";
            string fileNameToDelete = "";

            if (file != null && file.Length != 0)
            {
                var DirName = Path.Combine(_configuration["MasterPath"].ToString(), "LeaveRule");
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                sFileName = file.FileName;
                sFileName = sFileName.Replace(" ", "");
                string Extension = Path.GetExtension(sFileName);

                string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(sFileName);
                string sfilewithExtension = FileNameWithoutExtension + Extension;
                fileNameToDelete = Guid.NewGuid().ToString() + sfilewithExtension;
                string excelPath = DirName + "\\" + fileNameToDelete;

                using (var stream = new FileStream(excelPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(excelPath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    leaveruleDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return leaveruleDetails;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    leaveruleDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return leaveruleDetails;
                }

                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];


                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "proc_Qzone_bulk_LeaveRule";
                var parameters = new DynamicParameters();

                parameters.Add("@Xmldoc", xmlInput);
                parameters.Add("@Created_By", User);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Leave Rule data uploaded successfully."))
                        {
                            leaveruleDetails.response = res;
                        }
                        else
                        {
                            leaveruleDetails.response = "Failed to import.";
                            leaveruleDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        leaveruleDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    leaveruleDetails.response = "Failed";
                }

            }
            else
            {
                leaveruleDetails.response = "File not found";
            }
            return leaveruleDetails;
        }

        public async Task<List<LeaveTypes>> GetLeaveType(string CompanyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@company_ID", CompanyId);

            var res = await this._dbRepository.GetItemsAsync("Proc_GetLeaveMasterType1", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<LeaveTypes>>(res) ?? new List<LeaveTypes>();
            }

            return new List<LeaveTypes>();
        }

        public async Task<LeaveRuleResponse> SaveUpdateDeleteLeaveRule([FromBody] LeaveRuleRequest request)
        {
            LeaveRuleResponse leaveruleDetails = new LeaveRuleResponse();

            if (request == null || request.leaverulemaster == null || !request.leaverulemaster.Any())
            {
                leaveruleDetails.response = "Invalid timesheet request.";
            }

            var xmlInput = BuildLeaveRuleXml(request);

            string storeProcedure = "Proc_QZONE_Create_Update_Delete_LeaveRule";
            var parameters = new DynamicParameters();

            parameters.Add("@Xmldoc", xmlInput);
            parameters.Add("@Created_By", request.Created_By);
            parameters.Add("@Mode", request.Mode);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(res) && (res.Contains("Leave Rules data Added successfully.") ||
                        res.Contains("Leave Rules data Updated successfully.") ||
                        res.Contains("Leave Rules data Deleted successfully.")))
                    {
                        leaveruleDetails.response = res;
                    }
                    else
                    {
                        leaveruleDetails.response = "Failed to " + request.Mode + ".";
                        leaveruleDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    leaveruleDetails.response = "Error while processing response.";
                }
            }
            else
            {
                leaveruleDetails.response = "Failed";
            }

            return leaveruleDetails;
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

        private string BuildLeaveRuleXml(LeaveRuleRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<NewDataSet>");

            foreach (var row in request.leaverulemaster)
            {
                sb.Append("<LeaveRule>");
                sb.AppendFormat("<SrNo>{0}</SrNo>", row.SrNo);
                sb.AppendFormat("<Company_ID>{0}</Company_ID>", row.Company_ID);
                sb.AppendFormat("<Company_Name>{0}</Company_Name>", row.Company_Name);
                sb.AppendFormat("<State>{0}</State>", row.State);
                sb.AppendFormat("<Group_name>{0}</Group_name>", row.Group_name);
                sb.AppendFormat("<GroupDetail_Id>{0}</GroupDetail_Id>", row.GroupDetail_Id);
                sb.AppendFormat("<EffectiveDate>{0}</EffectiveDate>", row.EffectiveDate);
                sb.AppendFormat("<Start_Month>{0}</Start_Month>", row.Start_Month);
                sb.AppendFormat("<End_Month>{0}</End_Month>", row.End_Month);
                sb.AppendFormat("<leavetype>{0}</leavetype>", row.leavetype);
                sb.AppendFormat("<Invoice_leavedetails>{0}</Invoice_leavedetails>", row.Invoice_leavedetails);
                sb.AppendFormat("<Leavedetails>{0}</Leavedetails>", row.Leavedetails);
                sb.AppendFormat("<Carryforward>{0}</Carryforward>", row.Carryforward);
                sb.AppendFormat("<MaxDays>{0}</MaxDays>", row.MaxDays);
                sb.AppendFormat("<Encashment>{0}</Encashment>", row.Encashment);
                sb.Append("</LeaveRule>");
            }

            sb.Append("</NewDataSet>");
            return sb.ToString();
        }

    }
}
