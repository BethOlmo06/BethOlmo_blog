using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BethOlmo_blog.Startup))]
namespace BethOlmo_blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
