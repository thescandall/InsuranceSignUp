using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarInsuranceSignUp.Startup))]
namespace CarInsuranceSignUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
