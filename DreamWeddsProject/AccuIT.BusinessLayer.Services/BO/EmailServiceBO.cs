using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class EmailServiceBO
    {
        public long EmailServiceID { get; set; }
        public int TemplateID { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public bool IsAttachment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string AttachmentFileName { get; set; }
        public string Remarks { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string MEssage { get; set; }
        public string Location { get; set; }
    }
}
