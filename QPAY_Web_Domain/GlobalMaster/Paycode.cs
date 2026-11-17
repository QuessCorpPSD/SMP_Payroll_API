using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.GlobalMaster
{

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "PayCodesDetails")]
    [Serializable()]
    public class PayCodesResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("PayCodes")]
        public PayCodes[] PayCodesDetails { get; set; }
    }


    [Table("tbl_Paycode")]
    public class PayCodes
    {
        public int Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Description { get; set; }
        public string Print_As { get; set; }
        public int PayType_Id { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsProjectTax { get; set; }
        public bool IsMarginalTax { get; set; }
        public bool Paycode_Type { get; set; }
        public bool Is_LOP_Applicable { get; set; }
        public bool Is_PF_Applicable { get; set; }
        public bool Is_ESI_Applicable { get; set; }
        public bool Is_PT_Applicable { get; set; }

        public int Page_Type { get; set; }

        public int Account_Number { get; set; }
        public int Posting_key { get; set; }

        public int Tax_Type_ID { get; set; }
        
    }

    [Table("tbl_PayType")]
    public class PayTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayType_Id { get; set; }

        public string PayType { get; set; }
    }

    [Table("tbl_Page_Type")]
    public class PageType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Page_Type_Id { get; set; }

        public string Page_Type { get; set; }
    }

    public class PaycodeSearchParams
    {
        public string? paycode_Code { get; set; }
        public int? PayTypeId { get; set; }
        public int? IsTaxable { get; set; }
        public int? PayId { get; set; }
    }

    public class PaycodeCreateParams
    {
        //public string strXmlDetails { get; set; }
        
        public string mode { get; set; }
        public int userId { get; set; }

        public PayCodes detail { get; set; }

    }

}
