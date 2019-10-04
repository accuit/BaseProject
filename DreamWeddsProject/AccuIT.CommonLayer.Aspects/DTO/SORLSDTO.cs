using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public  class SORLSDTO
    {
        [DataMember]
        public string ClaimNo { get; set; }
        [DataMember]
        public string ClaimDate { get; set; }
        [DataMember]
        public string BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public string AscCode { get; set; }
        [DataMember]
        public string GcicStatus { get; set; }
        [DataMember]
        public string GCICReasonDescription { get; set; }
        [DataMember]
        public string RlsStatus { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public string SerialNoIMEI { get; set; }
        [DataMember]
        public string RECEIVED_DT { get; set; }
        [DataMember]
        public string CLOSE_DATE { get; set; }
        [DataMember]
        public string SawNo { get; set; }
        [DataMember]
        public string SawStatus { get; set; }
        [DataMember]
        public string SawDateTime { get; set; }
        [DataMember]
        public string DEFECT_DESC { get; set; }
        [DataMember]
        public string RepairDesc { get; set; }
        [DataMember]
        public string StatusID { get; set; }
        [DataMember]
        public string RejectReason { get; set; }
        [DataMember]
        public string RejectRemarks { get; set; }
        [DataMember]
        public string BPNAME { get; set; }
        
    }
}
