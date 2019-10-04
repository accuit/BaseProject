using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationLayer.DreamWedds.Web.Models;
using System.Text;
using System.Web.UI;
using AccuIT.BusinessLayer.Services.BO;

namespace PresentationLayer.DreamWedds.Web.Models
{

     
    public class DreamweddsModel
    {
        public TemplateMasterBO templateMaster { get; set; }
        public List<TemplateMasterBO> templateMasters { get; set; }

        public DreamWeddsBlogBO blog { get; set; }
        public List<DreamWeddsBlogBO> blogs { get; set; }

        public List<FAQBO> faqs { get; set; }
    }
       
}