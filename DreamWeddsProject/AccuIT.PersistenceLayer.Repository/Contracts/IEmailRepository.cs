using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuIT.PersistenceLayer.Repository.Entities;
using AccuIT.CommonLayer.Aspects.Utilities;

namespace AccuIT.PersistenceLayer.Repository.Contracts
{
    public interface IEmailRepository
    {
        TemplateMaster GetEmailTemplate(int TemplateCode);

        List<TemplateMergeField> GetEmailMergeFields(int templateCode);

        bool InsertEmailRecord(EmailService email);
    }
}
