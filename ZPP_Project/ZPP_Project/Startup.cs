using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZPP_Project.Startup))]
namespace ZPP_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
