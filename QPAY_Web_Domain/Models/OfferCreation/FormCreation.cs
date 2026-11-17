using QPay.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.OfferCreation
{
    public class FormCreation
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Field
        {
            public string fieldId { get; set; }
            public string fieldName { get; set; }
            public string labelName { get; set; }
            public string fieldType { get; set; }
            public Boolean isRequired { get; set; }
            public string defaultValue { get; set; }
            public Boolean isApiCall { get; set; }
            public string apiUrl { get; set; }
            public string options { get; set; }
            public Int32 parentId { get; set; }
            public Int32 displayOrder { get; set; }
        }

        public class Form
        {
            public string formId { get; set; }
            public string formName { get; set; }
            public string formGroupId { get; set; }
            public string clientId { get; set; }
            public string entityId { get; set; }
            public string siteId { get; set; }
            public Boolean isActive { get; set; }
            public List<Field> fields { get; set; }
        }

        public class FormResponse
        {
            public string formGroup { get; set; }
            public List<Form> forms { get; set; }
        }


    }
}
