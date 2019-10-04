using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.DreamWedds.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
