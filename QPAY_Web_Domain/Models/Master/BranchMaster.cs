using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Master
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "BranchMasterDetails")]
    [System.Serializable()]
    public class BranchMasterResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("BranchMaster")]
        public BranchMaster[] BranchMasterDetails { get; set; }
    }
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class BranchMaster
    {
        public int Branch_Id { get; set; }
        public string Branch_Name { get; set; }
        public int? PT_State_Id { get; set; }
        public string PT_State { get; set; }
        public int SNo { get; set; }
        public string Error_Message { get; set; }
    }
    public class BranchMasterXml
    {
        public BranchMaster details { get; set; }

        public string mode { get; set; }
        public int userId { get; set; }


    }

    public class BranchSearchRequest
    {
        public int? StateId { get; set; }
        public string? BranchName { get; set; }
    }

}
