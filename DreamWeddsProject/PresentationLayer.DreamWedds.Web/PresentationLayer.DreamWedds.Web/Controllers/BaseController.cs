using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationLayer.DreamWedds.Web.Models;
using System.Text;
using System.Web.UI;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.BusinessLayer.Services.BO;

namespace PresentationLayer.DreamWedds.Web.Controllers
{
    [OutputCache(Duration = 10, VaryByParam = "none",
    Location = OutputCacheLocation.Client, NoStore = true)]
    public class BaseController : Controller
    {
        private ISystemService systemBusinessInstance;
        private IUserService userBusinessInstance;
        private ISecurityService securityBusinessInstance;
        private IWeddingService weddingBusinessInstance;
        private IEmailService emailBusinessInstance;
        private int userID;
        private int roleID;
        private UserProfileBO userProfile;

        public class UniqueEmployee
        {
            public int? UserID;
        }

        #region Properties

        public int UserID
        {
            get { return (int)HttpContext.Session["UserID"]; }
            set { userID = value; }
        }

        public UserProfileBO UserProfile
        {
            get { return (UserProfileBO)HttpContext.Session["UserProfile"]; }
            set { userProfile = value; }
        }
        public int USERRoleID
        {
            get { return (int)HttpContext.Session["RoleID"]; }
            set { roleID = value; }
        }

        public ISystemService SystemBusinessInstance
        {
            get
            {
                if (systemBusinessInstance == null)
                {
                    systemBusinessInstance = AopEngine.Resolve<ISystemService>(AspectEnums.AspectInstanceNames.ServiceManager, AspectEnums.ApplicationName.AccuIT);
                }
                return systemBusinessInstance;
            }
        }
        public ISecurityService SecurityBusinessInstance
        {
            get
            {
                if (securityBusinessInstance == null)
                {
                    securityBusinessInstance = AopEngine.Resolve<ISecurityService>(AspectEnums.AspectInstanceNames.SecurityManager, AspectEnums.ApplicationName.AccuIT);
                }
                return securityBusinessInstance;
            }
        }
        public IUserService UserBusinessInstance
        {
            get
            {
                if (userBusinessInstance == null)
                {
                    userBusinessInstance = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.AccuIT);
                }
                return userBusinessInstance;
            }
        }
        public IWeddingService WeddingBusinessInstance
        {
            get
            {
                if (weddingBusinessInstance == null)
                {
                    weddingBusinessInstance = AopEngine.Resolve<IWeddingService>(AspectEnums.AspectInstanceNames.WeddingManager, AspectEnums.ApplicationName.AccuIT);
                }
                return weddingBusinessInstance;
            }
        }
        public string LastMinute
        {
            get { return " 11:00 PM"; }

        }

        #endregion

       
        DreamweddsModel dreamweddsModel = new DreamweddsModel();
        private List<TemplateMasterBO> templateMaster;
        private List<DreamWeddsBlogBO> dreamweddsBlogs;
        
        public List<TemplateMasterBO> WebTemplates
        {
            get { return templateMaster; }
            set { templateMaster = value; }
        }

        public List<DreamWeddsBlogBO> WebBlogs
        {
            get { return dreamweddsBlogs; }
            set { dreamweddsBlogs = value; }
        }

       
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            try
            {
                #region  Validation for multiple session

                var commmonsetup =  SystemBusinessInstance.GetCommonSetup(0, null, Convert.ToString((int)AspectEnums.CommonTableMainType.Template));// DBEntities.CommonSetups.Where(x => x.ParentID == 1).ToList();

                //var templates = DBEntities.TemplateMasters.Where(x => x.TemplateType == (int)AspectEnums.TemplateType.Wedding && x.TemplateStatus == 1).ToList();
                List<TemplateMasterBO> templates =  SystemBusinessInstance.GetAllTemplates((int)AspectEnums.TemplateType.Wedding);
                foreach (var item in templates)
                {
                    StringBuilder tagName = new StringBuilder();
                    StringBuilder feature = new StringBuilder();
                    StringBuilder tagid = new StringBuilder();
                    foreach (var tag in item.TemplateTags.Split(','))
                    {

                        string strtag = commmonsetup.Where(x => x.SubType == "Tag" && x.DisplayValue == Convert.ToInt32(tag)).First().DisplayText;
                        tagName.Append(strtag).Append(',').Append(' ');
                    }
                    item.TemplateTagName = tagName.ToString().Trim(',').Trim(' ');

                    if (item.Features != null)
                    {
                        foreach (var tempfeature in item.Features.Split(','))
                        {

                            string strfeature = commmonsetup.Where(x => x.SubType == "Features" && x.DisplayValue == Convert.ToInt32(tempfeature)).First().DisplayText;
                            feature.Append(strfeature).Append(',').Append(' ');
                        }
                        item.TemplateFeatureText = feature.ToString().Trim(',').Trim(' ');
                    }
                    item.TemplateTypeText = commmonsetup.Where(x => x.SubType == "Type" && x.DisplayValue == item.TemplateType).First().DisplayText;

                }

                dreamweddsModel.templateMasters = templates;
                WebTemplates = templates;

                if (WebTemplates != null)
                {
                    requestContext.HttpContext.Session["Templates"] = WebTemplates;
                }
                #endregion

                WebBlogs = WeddingBusinessInstance.GetDreamWeddsBlog();
                dreamweddsModel.blogs = WebBlogs;
                if (WebBlogs != null)
                {
                    requestContext.HttpContext.Session["Blogs"] = dreamweddsModel.blogs;
                }
                base.Initialize(requestContext);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

        }
    }
}