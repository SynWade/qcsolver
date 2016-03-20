using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(qcsolver.Startup))]
namespace qcsolver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
