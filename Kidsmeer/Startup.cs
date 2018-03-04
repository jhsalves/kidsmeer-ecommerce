using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kidsmeer.Startup))]
namespace Kidsmeer
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
