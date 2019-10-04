using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PresentationLayer.DreamWedds.Web.Models
{

    public class DreamWeddsData
    {

        #region private fields initializatioh
        private ISystemService systemBusinessInstance;
        private IUserService empBusinessInstance;
        private IWeddingService weddingBusinessInstance;
      
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
        public IUserService UserBusinessInstance
        {
            get
            {
                if (empBusinessInstance == null)
                {
                    empBusinessInstance = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.AccuIT);
                }
                return empBusinessInstance;
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

#endregion

        public static DreamweddsModel DreamWeddsWeb;

        private static List<TemplateMasterBO> templateMaster;
        private static List<DreamWeddsBlogBO> dreamweddsBlogs;
        private static List<FAQBO> faqs;

        public  List<TemplateMasterBO> Templates
        {
            get { return templateMaster; }
            set { templateMaster = value; }
        }

        public List<DreamWeddsBlogBO> Blogs
        {
            get { return dreamweddsBlogs; }
            set { dreamweddsBlogs = value; }
        }

        public List<FAQBO> Faqs
        {
            get { return faqs; }
            set { faqs = value; }
        }

        [OutputCache(Duration = 9999 * 999, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public DreamweddsModel GetWebsiteData()
        {
           
            DreamweddsModel dreamweddsModel = new DreamweddsModel();
            
            try
            {
                #region  Validation for multiple session

                var commmonsetup = SystemBusinessInstance.GetCommonSetup(0, null, Convert.ToString((int)AspectEnums.CommonTableMainType.Template));// DBEntities.CommonSetups.Where(x => x.ParentID == 1).ToList();

                var templates = SystemBusinessInstance.GetAllTemplates(null);
                foreach (var item in templates.Where(x=>x.TemplateType == (int)AspectEnums.TemplateType.Wedding))
                {
                    StringBuilder tagName = new StringBuilder();
                    StringBuilder feature = new StringBuilder();
                    StringBuilder tagid = new StringBuilder();
                    //foreach (var tag in item.TemplateTags.Split(','))
                    //{
                    //    string strtag = commmonsetup.Where(x => x.SubType == "Tag" && x.DisplayValue == Convert.ToInt32(tag)).First().DisplayText;
                    //    tagName.Append(strtag).Append(',').Append(' ');
                    //}
                   // item.TemplateTagName = tagName.ToString().Trim(',').Trim(' ');

                    if (item.Features != null)
                    {
                        foreach (var tempfeature in item.Features.Split(','))
                        {

                            string strfeature = commmonsetup.Where(x => x.SubType == "Features" && x.DisplayValue == Convert.ToInt32(tempfeature)).FirstOrDefault().DisplayText;
                            feature.Append(strfeature).Append(',').Append(' ');
                        }
                        item.TemplateFeatureText = feature.ToString().Trim(',').Trim(' ');
                    }
                    item.TemplateTypeText = commmonsetup.Where(x => x.SubType == "Type" && x.DisplayValue == item.TemplateType).First().DisplayText;

                }

                dreamweddsModel.templateMasters = templates;
                Templates = templates;

                #endregion

                Blogs = WeddingBusinessInstance.GetDreamWeddsBlog();
                dreamweddsModel.blogs = Blogs;

                Faqs = WeddingBusinessInstance.GetDreamWeddsFAQ();
                dreamweddsModel.faqs = Faqs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            DreamWeddsWeb = dreamweddsModel;
            return dreamweddsModel;

        }

    }
}