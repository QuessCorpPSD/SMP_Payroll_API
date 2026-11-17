using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Master
{

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "TDSSlabDetailMaster")]
    [System.Serializable()]
    public class TDSSlabDetailMasterWrappernew
    {
        [System.Xml.Serialization.XmlElementAttribute("TDSSlabMaster")]
        public TDSSlabMaster[] TDSSlabMaster { get; set; }
    }
    public class TDSSlabMaster
    {
        public int TDS_Slab_Id { get; set; }
        public string TDS_Category { get; set; }
    }
    public class TDSSlabDetailMaster
    {
        public int TDS_Slab_Id { get; set; }
        public int TDS_Slab_Detail_Id { get; set; }
        public decimal Income_From { get; set; }
        public decimal Income_To { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax_Percentage { get; set; }
    }

    public class TDSSlabRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public TDSSlabMaster parentDetail { get; set; }
        public List<TDSSlabDetailMaster> ChildDetail { get; set; }

    }

    [XmlRoot("TDSSlabsDetails")]
    public class TDSSlabMasterWrapper
    {
        public TDSSlabMaster TDSSlab { get; set; }
    }

    [XmlRoot("TDSSlabDetailResponse")]
    public class TDSSlabDetailMasterWrapper
    {
        [XmlElement("TDSSlabDetail")]
        public List<TDSSlabDetailMaster> TDSSlabDetailMaster { get; set; }
    }
}
