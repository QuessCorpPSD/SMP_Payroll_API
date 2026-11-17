using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Admin
{
    //public class CompanyPermission
    //{
    //    public List<CompanyPermissionModelDetails> CompanyPermissionDetails { get; set; }

    //    public string vmode { get; set; }

    //    public int userId { get; set; }
    //}

    //public class CompanyPermissionModelDetails
    //{
    //    public int Company_Permission_Details_Id { get; set; }
    //    public int Company_Permission_Id { get; set; }
    //    public string Is_Permission { get; set; }
    //    public int Company_Id { get; set; }
    //    public string Company_Code { get; set; }
    //    public string Company_Name { get; set; }
    //    public string Zone_Name { get; set; }
    //    public string Access { get; set; }
    //}

    [XmlRoot("CompanyPermissionResponse")]
    public class CompanyPermissionRequest
    {
        [XmlIgnore]
        public int createdBy { get; set; }

        [XmlIgnore]
        public string mode { get; set; }

        [XmlElement("CompanyPermissionModelResponse")]
        public CompanyPermissionModel CompanyPermissionModel { get; set; }

        [XmlElement("CompanyPermissionModelDetailsResponse")]
        public List<CompanyPermissionDetails> CompanyPermissionDetails { get; set; }
    }

    public class CompanyPermissionModel
    {
        [XmlElement("User_Id")]
        public int? User_Id { get; set; }

        [XmlElement("Business_Unit_Name_id")]
        public int? Business_Unit_Name_id { get; set; }
        [XmlElement("Company_Permission_Id")]
        public int? Company_Permission_Id { get; set; }
    }

    public class CompanyPermissionDetails
    {
        [XmlElement("Company_Permission_Details_Id")]
        public int? Company_Permission_Details_Id { get; set; }

        [XmlElement("Company_Permission_Id")]
        public int? Company_Permission_Id { get; set; }

        [XmlElement("Is_Permission")]
        public bool? Is_Permission { get; set; }

        [XmlElement("Company_Id")]
        public int? Company_Id { get; set; }

        [XmlElement("Company_Code")]
        public string? Company_Code { get; set; }
    }

    public class LoadCompany
    {

        public int? BusinessUnitNameId { get; set; }
        public int? BusinessZonenName { get; set; }


    }

    public class EditPermissiondetails
    {

        public int? Userid { get; set; }
        public int? Businessunitnameid { get; set; }
        public int? CompanyPermissionId { get; set; }


    }


}
