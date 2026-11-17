using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Master
{

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ComputationRuleDetails")]
    [System.Serializable()]
    public class ComputationRuleResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ComputationRule")]
        public ComputationRule[] ComputationRuleDetails { get; set; }
    }
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ComputationRule
    {
        public int Computation_Rule_Id { get; set; }
        public int Financial_Year_Id { get; set; }
        public string Tax_Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Error_Message { get; set; }
        public string Financial_Year_Name { get; set; }
        public string Computation_Rule { get; set; }
        public int SNo { get; set; }
        public DateTime Effective_Date { get; set; }
    }
    public class ComputationRuleXml
    {
        public ComputationRule details { get; set; }

        public string mode { get; set; }
        public int userId { get; set; }


    }

    public class ComputationSearch
    {
        public int? FinancialYearId { get; set; }
        public string? Category { get; set; }
        public string? TaxId { get; set; }

    }
}
