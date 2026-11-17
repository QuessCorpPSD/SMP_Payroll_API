using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.OfferCreation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.GlobalMaster
{

    public class FormCreationRepository : IFormCreationRepository
    {
        private readonly DbRepository _dbRepository;

        public FormCreationRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<FormCreation.FormResponse> Search(int? FormId, int? ClientId, int? EntityId, int? SiteId)
        {
            var parameters = new Dictionary<string, object>
            {
                //["@FormId"] = FormId,
                //["@ClientId"] = ClientId,
                //["@EntityId"] = EntityId,
                //["@SiteId"] = SiteId,
            };

            DataSet data = _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Get_FormCreation", parameters);

            FormCreation.FormResponse response=new FormCreation.FormResponse();
            if (data.Tables.Count > 0)
            {
                response.formGroup = data.Tables[0].Rows[0]["FormGroup"].ToString();
                response.forms = new List<FormCreation.Form>();
                foreach (DataRow row in data.Tables[1].Rows)
                {
                    FormCreation.Form form = new FormCreation.Form
                    {
                        formId = row["FormId"].ToString(),
                        formName = row["FormName"].ToString(),
                        formGroupId = row["FormGroupId"].ToString(),
                        clientId = row["ClientId"].ToString(),
                        entityId = row["EntityId"].ToString(),
                        siteId = row["SiteId"].ToString(),
                        isActive = Convert.ToBoolean(row["IsActive"]),
                        fields = new List<FormCreation.Field>()
                    };
                    DataRow[] fieldRows = data.Tables[2].Select($"FormId = {form.formId}");
                    foreach (DataRow fieldRow in fieldRows)
                    {
                        FormCreation.Field field = new FormCreation.Field
                        {
                            fieldId = fieldRow["FieldId"].ToString(),
                            fieldName = fieldRow["FieldName"].ToString(),
                            labelName = fieldRow["LabelName"].ToString(),
                            fieldType = fieldRow["FieldType"].ToString(),
                            isRequired = Convert.ToBoolean(fieldRow["IsRequired"]),
                            defaultValue = fieldRow["DefaultOption"].ToString(),
                            isApiCall = Convert.ToBoolean(fieldRow["IsApiCall"]),
                            apiUrl = fieldRow["ApiUrl"].ToString(),
                            options = fieldRow["Options"].ToString(),
                            parentId = Convert.ToInt32(fieldRow["ParentId"]),
                            displayOrder = Convert.ToInt32(fieldRow["DisplayOrder"])
                        };
                        form.fields.Add(field);
                    }
                    response.forms.Add(form);
                }
            }
            return response;
        }


    }
}
