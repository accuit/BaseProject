using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PresentationLayer.DreamWedds.Web.Startup))]
namespace PresentationLayer.DreamWedds.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
