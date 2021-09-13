using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(University.web.Startup))]
namespace University.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
