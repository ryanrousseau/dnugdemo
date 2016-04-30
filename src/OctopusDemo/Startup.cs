using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LRTechFestDemo.Startup))]
namespace LRTechFestDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
