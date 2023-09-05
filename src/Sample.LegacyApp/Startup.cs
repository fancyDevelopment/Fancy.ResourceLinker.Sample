using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sample.LegacyApp.Startup))]
namespace Sample.LegacyApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
