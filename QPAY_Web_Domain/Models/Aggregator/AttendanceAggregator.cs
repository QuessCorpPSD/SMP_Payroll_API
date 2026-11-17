using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Aggregator
{
    public class AttendanceAggregator
    {
        public int LEAVE_MAPPING_DETAIL_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public int SITEID { get; set; }
        public int LEAVE_TYPE_ID { get; set; }
        public int LEAVE_TREAT_ID { get; set; }
        public Boolean ISACTIVE { get; set; }
        public int ATTENDANCE_TYPE { get; set; }
        public int LEAVE_POLICY_ID { get; set; }
        public string? BUFFER_DATE { get; set; } = "";
    }


    public class AttendanceAggregatorRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public AttendanceAggregator parentDetail { get; set; }

    }

    public class leaveTypeMaster
    {
        public int LEAVE_TYPE_ID { get; set; }
        public string LEAVE_TYPE_NAME { get; set; }
        public string LEAVE_TYPE_DESCRIPTION { get; set; }
        public Boolean ISACTIVE { get; set; }
    }

    public class leaveTypeMasterRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public leaveTypeMaster parentDetail { get; set; }

    }


    public class AttributesMapping
    {
        public int company_Id { get; set; }
        public int Template_Field_Id { get; set; }
        public int Quess_Template_Field_Id { get; set; }
        public Boolean IsActive { get; set; }
    }


    public class AttributesMappingRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public AttributesMapping parentDetail { get; set; }

    }

    public class RequestResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }


    public class ResponseModel
    {
        public string? Result { get; set; }
        public string? Error_Message { get; set; }
        public string? Validation { get; set; }
    }


    public class MISCPaycodeMapping
    {
        public int Company_Id { get; set; }
        public int SiteId { get; set; }
        public string FeeCode { get; set; } = string.Empty;
        public string Pay_Code { get; set; } = string.Empty;
        public int Pay_Code_Id { get; set; }
    }


    public class MISCPaycodeMappingRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public MISCPaycodeMapping parentDetail { get; set; }

    }


}
