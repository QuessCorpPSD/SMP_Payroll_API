using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.PurchaseOrder;
using QPay.DAL.Repository;
using System.Data;
using System.Reflection;
using static QPay.IRepository.Repository.PurchaseOrder.PurchaseOrderRepository;
using static QPay.UI_Domain.Models.PurchaseOrder.EmpPO;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;

namespace QPay.IRepository.Repository.PurchaseOrder
{
    public class EmployeePOApproveRepository : IEmployeePOApproveRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public EmployeePOApproveRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public DataSet EmployeePOApproveSearch(string companyId, string siteId, string poNumber, string pricingType,
            string employeeId, string status)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@COMPANY_ID"] = companyId,
                ["@SITEID"] = siteId,
                ["@PONUMBER"] = poNumber,
                ["@Pricingtype"] = pricingType,
                ["@EMPLOYEEID"] = employeeId,
                ["@STATUS"] = status
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_PO_VIEW_APPROVE_NEW", parameters, 1500);
        }

        public PoResponse EmployeePOApproveReject(List<EmployeePOApproveReject> request, string Createdby)
        {
            PoResponse PoDetails = new PoResponse();

            if (request == null)
            {
                PoDetails.response = "Invalid Po Request";
                return PoDetails;
            }

            DataSet ds = new DataSet("DocumentElement");
            ds = ToDataSet(request);
            //Convert dt to XML
            if (ds.Tables.Count == 0)
            {
                PoDetails.response = "Po data is not available";
                return PoDetails;
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

            string storeProcedure = "USP_PO_UPDATE_APPREJ";
            var parameters = new DynamicParameters();

            parameters.Add("@XML", xmlInput);
            parameters.Add("@CreatedBy", Createdby);

            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("SUCCESSFULLY  APPROVED"))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("PO REJECTED"))
                    {
                        PoDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("PO REVOKED"))
                    {
                        PoDetails.response = message;
                    }
                    else
                    {
                        PoDetails.response = "Failed to import.";
                        PoDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    PoDetails.response = "Error while processing response.";
                }
            }
            else
            {
                PoDetails.response = "Failed";
            }
            return PoDetails;
        }

        public DataSet EmployeePOExporttoexcel(string companyId, string siteId, string poNumber, string pricingType,
            string employeeId, string status)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@COMPANY_ID"] = companyId,
                ["@SITEID"] = siteId,
                ["@PONUMBER"] = poNumber,
                ["@Pricingtype"] = pricingType,
                ["@EMPLOYEEID"] = employeeId,
                ["@STATUS"] = status
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_PO_VIEW_APPROVE_Export_Excel", parameters, 1500);
        }

        public PoEmpDurationUI GetPoEmpDuration(EmployeePODurationRequest request)
        {
            const string storedProcedure = "[dbo].[USP_PO_EMP_DURATIONCAL_NewUI]";

            var parameters = new DynamicParameters();
            parameters.Add("@POID", request.PO_ID);
            parameters.Add("@Startdate", request.Startdate);
            parameters.Add("@ENDDATE", request.ENDDATE);

            var res = _dbRepository.GetItemsAsync(storedProcedure, parameters).Result;

            if (string.IsNullOrWhiteSpace(res))
            {
                return new PoEmpDurationUI(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<PoEmpDurationUI>>(res);
                return list?.FirstOrDefault() ?? new PoEmpDurationUI();
            }
            catch (JsonException ex)
            {
                return new PoEmpDurationUI();
            }
        }

        public PoEmpCalculationUI GetPoEmpCalculation(EmployeePOCalculationRequest request)
        {
            const string storedProcedure = "[dbo].[USP_PO_EMP_CALCULATION_NewUI]";

            var parameters = new DynamicParameters();
            parameters.Add("@POID", request.PO_ID);
            parameters.Add("@CLIENTEMPID", request.CLIENTEMPID);
            parameters.Add("@PRICINGTYPE", request.PRICINGTYPE);
            parameters.Add("@Startdate", request.Startdate);
            parameters.Add("@ENDDATE", request.ENDDATE);
            parameters.Add("@QUANTITYTYPE", request.QUANTITYTYPE);

            var res = _dbRepository.GetItemsAsync(storedProcedure, parameters).Result;

            if (string.IsNullOrWhiteSpace(res))
            {
                return new PoEmpCalculationUI(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<PoEmpCalculationUI>>(res);
                return list?.FirstOrDefault() ?? new PoEmpCalculationUI();
            }
            catch (JsonException ex)
            {
                return new PoEmpCalculationUI();
            }
        }

        public PoEmpValueUI GetPoEmpValue(EmployeePOValueRequest request)
        {
            const string storedProcedure = "[dbo].[USP_PO_EMPVALUE_NewUI]";

            var parameters = new DynamicParameters();
            parameters.Add("@QUANTITYTYPE", request.QUANTITYTYPE);
            parameters.Add("@QUANTITY", request.QUANTITY);
            parameters.Add("@FIXEDRATE", request.FIXEDRATE);

            var res = _dbRepository.GetItemsAsync(storedProcedure, parameters).Result;

            if (string.IsNullOrWhiteSpace(res))
            {
                return new PoEmpValueUI(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<PoEmpValueUI>>(res);
                return list?.FirstOrDefault() ?? new PoEmpValueUI();
            }
            catch (JsonException ex)
            {
                return new PoEmpValueUI();
            }
        }

        public static DataSet ToDataSet<T>(List<T> items)
        {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable("Table");

            // Get all public properties of T
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                table.Rows.Add(values);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }
    }
}
