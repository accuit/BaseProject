using System.Web;
using System.Web.Mvc;

namespace AccuIT.PresentationLayer.WebAdmin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}