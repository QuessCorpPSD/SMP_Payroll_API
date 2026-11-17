using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Master;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QPay.BAL.Repository.Master
{
    public class TDSSlabMasterRepository : ITDSSlabMasterRepository
    {
        private readonly DbRepository _dbRepository;

        public TDSSlabMasterRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<DataSet> GetFinancialYear()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@FinancialYearId"] = "",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetFinancialYear", parameters); ;

        }
        public async Task<DataSet> Search(string SearchCategory, int? tdsSlabId, string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@TDS_Category"] = SearchCategory,
                ["@TDS_Slab_Id"] = tdsSlabId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllTDSSlab", parameters); ;

        }

        public async Task<DataSet> GetAllTDSSlabDetail(int? tdsSlabId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@TDS_Slab_Id"] = tdsSlabId,
             
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllTDSSlabDetail", parameters); ;

        }

        public async Task<DataSet> Create([FromBody] TDSSlabRequest items, string schema)
        {

            var parentdatawrapper = new UI.Master.TDSSlabMasterWrapper
            {
                TDSSlab = new TDSSlabMaster
                {
                    TDS_Slab_Id = items.parentDetail.TDS_Slab_Id,
                    TDS_Category = items.parentDetail.TDS_Category
                }
            };

            var childdatawrapper = new UI.Master.TDSSlabDetailMasterWrapper
            {
                TDSSlabDetailMaster = items.ChildDetail.Select(child => new TDSSlabDetailMaster
                {
                    TDS_Slab_Id = child.TDS_Slab_Id,
                    TDS_Slab_Detail_Id = child.TDS_Slab_Detail_Id,
                    Income_From = child.Income_From,
                    Income_To = child.Income_To,
                    Amount = child.Amount,
                    Tax_Percentage = child.Tax_Percentage                   
                }).ToList()
            };

            string parentdata = ToXmlTDSSlab(parentdatawrapper);
            string childdata = ToXmlTDSSlabDetailNew(childdatawrapper);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@xmlInputDetail"] = childdata,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateTDSSlab", parameters);
        }

        public string ToXmlTDSSlabDetailNew(UI.Master.TDSSlabDetailMasterWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(UI.Master.TDSSlabDetailMasterWrapper));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // remove xmlns

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // 🚀 remove the XML header
                Indent = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, wrapper, ns);
                return sw.ToString();
            }
        }

        public string ToXmlTDSSlab(UI.Master.TDSSlabMasterWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(UI.Master.TDSSlabMasterWrapper));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // remove xmlns

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // 🚀 remove the XML header
                Indent = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, wrapper, ns);
                return sw.ToString();
            }
        }
        public async Task<DataSet> ExporttoExcel(int? FinancialYearId, string SearchCategory)
        {
            var parameters = new Dictionary<string, object>
            {

                ["@FinancialYear_Id"] = FinancialYearId,
                ["@TDS_Category"] = SearchCategory,
                
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spBranch_Search", parameters); ;
        }
    }
}
