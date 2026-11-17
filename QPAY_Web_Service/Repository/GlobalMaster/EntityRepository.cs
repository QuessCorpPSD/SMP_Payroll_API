using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.GlobalMaster
{

    public class EntityRepository : IEntityRepository
    {
        private readonly DbRepository _dbRepository;

        public EntityRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search(string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@EntityName"] = "",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_GetAllEntityProfitDetail", parameters); ;

        }

        public async Task<DataSet> GetQuessLegalEntity(string schema)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "GetQuessLegalEntity",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".USP_CommonDropDowns", parameters);

        }
        public async Task<DataSet> Create(EntityRequest items, string schema)
        {

            var entityXml = GenericSerializer<Entity>.Serialize(items.parentDetail);
            var parentdata = "";

            if (entityXml.ToString().Contains("EntityDetails"))
            {
                parentdata = entityXml;
            }
            else
            {
                parentdata = $"<EntityDetails>{entityXml}</EntityDetails>";
            }

            var ProfitCenterXml = GenericSerializer<EntityProfitCenter>.Serialize(items.ChildDetail);
            var childdata = "";

            if (ProfitCenterXml.ToString().Contains("EntityProfitCenterResponse"))
            {
                childdata = ProfitCenterXml;
            }
            else
            {
                childdata = $"<EntityProfitCenterResponse>{ProfitCenterXml}</EntityProfitCenterResponse>";
            }


            // var parentdata = GenericSerializer<Entity>.Serialize(items.parentDetail);
            //var childdata = GenericSerializer<EntityProfitCenter>.Serialize(items.ChildDetail);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@xmlInputDetail"] = childdata,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(schema + ".sp_CreateUpdateEntity", parameters);
        }


    }
}
