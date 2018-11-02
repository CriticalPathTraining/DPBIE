using Owin;
using System.Web.Http;
using System.Web.Routing;

namespace WorkspaceManager {

  public partial class Startup {
    public void Configuration(IAppBuilder app) {
      ConfigureAuth(app);
      GlobalConfiguration.Configure(WebApiConfig.Register);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
    }
  }

}
