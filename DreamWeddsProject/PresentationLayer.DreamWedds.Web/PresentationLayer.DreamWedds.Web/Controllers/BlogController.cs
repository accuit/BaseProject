using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationLayer.DreamWedds.Web.Models;
using System.Web.UI;

namespace PresentationLayer.DreamWedds.Web.Controllers
{
    public class BlogController : BaseController
    {
        
        DreamweddsModel M = new DreamweddsModel();
        DreamWeddsData D = new DreamWeddsData();

       // [OutputCache(Duration = 9999 * 999, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        [Route("our-blogs/all-blogs-list")]
        public ActionResult Index()
        {
            

            if (D.Blogs == null)
            {
                M.blogs = D.GetWebsiteData().blogs;
                M.templateMasters = D.GetWebsiteData().templateMasters;
            }
            else
            {
                M.blogs = D.Blogs;
                M.templateMasters = D.Templates;
            }
            return View(M);
        }

        [Route("{blogId:int}/{blogName}")]
        public ActionResult BlogDetail(int blogId, string blogName)
        {

            if (D.Blogs == null)
            {
               // M.blogs = D.GetWebsiteData().blogs;
                M.blog = D.GetWebsiteData().blogs.Where(x => x.BlogID == blogId).First();
                M.templateMasters = D.GetWebsiteData().templateMasters;
            }
            else
            {
                M.blogs = D.Blogs;
                M.blog = WeddingBusinessInstance.GetBlogDetails(blogId);
                M.templateMasters = D.Templates;
            }

                       
            return View(M);
        }
    }
}