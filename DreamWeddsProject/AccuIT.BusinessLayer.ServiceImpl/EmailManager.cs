using AccuIT.BusinessLayer.Base;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using AccuIT.CommonLayer.EntityMapper;

namespace AccuIT.BusinessLayer.ServiceImpl
{
    public class EmailManager : EmailBaseService, IEmailService
    {
        #region Properties

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.EMP_REPOSITORY)]
        public IUserRepository EmpRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.EMAIL_REPOSITORY)]
        public IEmailRepository EmailRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.SYSTEM_REPOSITORY)]
        public ISystemRepository SystemRepository { get; set; }

        #endregion


        /// <summary>
        /// Get Email Template based on ID
        /// </summary>
        /// <param name="TemplateCode">Template ID</param>
        /// <returns>Obejct of EmailTemplate</returns>
        public TemplateMasterBO GetEmailTemplate(int TemplateCode)
        {
            TemplateMasterBO emailTemplate = new TemplateMasterBO();
            ObjectMapper.Map(EmailRepository.GetEmailTemplate(TemplateCode), emailTemplate);
            return emailTemplate;
        }

        public List<TemplateMergeFieldBO> GetEmailMergeFields(int templateCode)
        {
            List<TemplateMergeFieldBO> fields = new List<TemplateMergeFieldBO>();
            ObjectMapper.Map(EmailRepository.GetEmailMergeFields(templateCode), fields);
            return fields;
        }

        public bool InsertEmailRecord(EmailServiceDTO email)
        {
            EmailService emailDetail = new EmailService();
            ObjectMapper.Map(email, emailDetail);
            return EmailRepository.InsertEmailRecord(emailDetail);
        }
    }
}
