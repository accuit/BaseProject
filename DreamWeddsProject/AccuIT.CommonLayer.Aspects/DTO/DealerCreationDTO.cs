using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class DealerCreationDTO
    {
        [DataMember]
        public int DealerCreationID { get; set; }
        [DataMember]
        public Nullable<int> REASONOFAPPOINTMENT { get; set; }
        [DataMember]
        public Nullable<int> TYPEOFFIRM { get; set; }
        [DataMember]
        public string NAMEOFFIRM { get; set; }
        [DataMember]
        public string DIVISIONCODE { get; set; }
        [DataMember]
        public string CHANNEL { get; set; }
        [DataMember]
        public string SUBCHANNELCODE { get; set; }
        [DataMember]
        public string CITYTOWN { get; set; }
        [DataMember]
        public string DISTRICT { get; set; }
        [DataMember]
        public string COUNTRY { get; set; }
        [DataMember]
        public string PINCODE { get; set; }
        [DataMember]
        public string STATE { get; set; }
        [DataMember]
        public string STREETNAME { get; set; }
        [DataMember]
        public string LANDLINENUMBER { get; set; }
        [DataMember]
        public string FIRMMOBILE { get; set; }
        [DataMember]
        public string FIRMEMAIL { get; set; }
        [DataMember]
        public string TYPEOFDEALER { get; set; }
        [DataMember]
        public string PARENTDEALERCODE { get; set; }
        [DataMember]
        public string PAN { get; set; }
        [DataMember]
        public string TIN { get; set; }
        [DataMember]
        public string NAMEOFOWNER { get; set; }
        [DataMember]
        public string MOBILEOFOWNER { get; set; }
        [DataMember]
        public string CONTACTPERSONNAME { get; set; }
        [DataMember]
        public string CONTPERSONMOBILE { get; set; }
        [DataMember]
        public string PARTNERTYPECODE { get; set; }
        [DataMember]
        public Nullable<int> DAYOFF { get; set; }
        [DataMember]
        public Nullable<int> APPROVALSTATUS { get; set; }
        [DataMember]
        public Nullable<int> TOTALCOUNTERSIZE { get; set; }
        [DataMember]
        public string STORESIZECODE { get; set; }
        [DataMember]
        public Nullable<decimal> LONGITUDE { get; set; }
        [DataMember]
        public Nullable<decimal> LATITUDE { get; set; }
        [DataMember]
        public string DMSPRMCODE { get; set; }
        [DataMember]
        public string LEGACYREFERENCENO { get; set; }
        [DataMember]
        public string SOURCE { get; set; }
        [DataMember]
        public Nullable<System.DateTime> MODIFIEDDATETIME { get; set; }
        [DataMember]
        public string MODIFIEDBY { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CREATEDDATETIME { get; set; }
        [DataMember]
        public string CREATEDBY { get; set; }
        [DataMember]
        public string TINPHOTO { get; set; }
        [DataMember]
        public string PANPHOTO { get; set; }
        [DataMember]
        public string GSBPHOTO { get; set; }
        [DataMember]
        public string OWNERPHOTO { get; set; }
        [DataMember]
        public string CONTACTPERSONPHOTO { get; set; }
        [DataMember]
        public string PROMOTERREQUIRED { get; set; }
        [DataMember]
        public string CONSUMERFINANCEAVAILABLE { get; set; }
        [DataMember]
        public string OWNERDOB { get; set; }
        [DataMember]
        public string PARENTCOMPANY { get; set; } 
    }
}
