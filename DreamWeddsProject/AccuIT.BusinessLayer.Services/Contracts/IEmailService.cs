using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Utilities;

namespace AccuIT.BusinessLayer.Services.Contracts
{
    public interface IEmailService
    {
        TemplateMasterBO GetEmailTemplate(int TemplateCode);
        List<TemplateMergeFieldBO> GetEmailMergeFields(int templateCode);
        bool InsertEmailRecord(EmailServiceDTO email);
    }
}
