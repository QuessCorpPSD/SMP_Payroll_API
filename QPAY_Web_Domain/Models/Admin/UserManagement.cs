using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Admin
{
    public class UserManagement
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? RoleId { get; set; }
        public int? IsCheck { get; set; }
    }

    [XmlRoot("UserResponse")]
    public class adminmenurequest
    {
            [XmlIgnore]
            public int createdBy { get; set; }
            [XmlIgnore]
            public string mode { get; set; }
            [XmlElement("UserDetails")]
            public UserDetails UserDetails { get; set; }

     }

    public class UserDetails
    {
            public int? User_Id { get; set; }

            public string? Name { get; set; }
            public string? Password { get; set; }
            public string? Salt { get; set; }
            public string? Mail_Id { get; set; }
            public int? Reporting_To { get; set; }
            public int? Role_Id { get; set; }
            public int? Access_Type_Id { get; set; }
            public int? EmployeeID { get; set; }
            public int? IsActive { get; set; }
        }
    }
