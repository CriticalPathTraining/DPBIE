using Owin;

namespace DailyReporterPersonal {
  public partial class Startup {
    public void Configuration(IAppBuilder app) {
      ConfigureAuth(app);
    }
  }
}