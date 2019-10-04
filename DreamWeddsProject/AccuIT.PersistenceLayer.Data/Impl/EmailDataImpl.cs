using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuIT.PersistenceLayer.Data.Impl
{
    public class EmailDataImpl : BaseDataImpl, IEmailRepository
    {
        public TemplateMaster GetEmailTemplate(int TemplateCode)
        {
            TemplateMaster template = AccuitAdminDbContext.TemplateMasters.FirstOrDefault(k => k.TemplateCode == TemplateCode && k.TemplateType == (int)AspectEnums.TemplateType.Email && k.TemplateStatus == (int)AspectEnums.TemplateStatus.Active && k.IsDeleted == false);
            return template;
        }

        public List<TemplateMergeField> GetEmailMergeFields(int templateCode)
        {
            List<TemplateMergeField> fields = new List<TemplateMergeField>();
            return AccuitAdminDbContext.TemplateMergeFields.Where(x => x.TemplateID == templateCode && x.IsDeleted == false).ToList();
        }

        public bool InsertEmailRecord(EmailService email)
        {
            bool isSuccess = false;
            email.CreatedDate = System.DateTime.Now;
            AccuitAdminDbContext.EmailServices.Add(email);
            isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
            return isSuccess;
        }

    }
}
