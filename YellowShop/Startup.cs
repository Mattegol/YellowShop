using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YellowShop.Startup))]
namespace YellowShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
