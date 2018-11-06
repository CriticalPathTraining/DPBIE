using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DailyReporterPersonal.Models;

namespace DailyReporterPersonal.Controllers {
  public class HomeController : Controller {

    public async Task<ActionResult> Index() {

      if (this.User.Identity.IsAuthenticated) {
        string json = await PowerBiEmbeddingManager.GetViewModelJSON();
        return View(new HomeViewModel { isUserAuthenticated = true, jsonViewModel = json });
      }
      else {
        return View(new HomeViewModel { isUserAuthenticated = false, jsonViewModel = "" });
      }
      
    }
  }
}