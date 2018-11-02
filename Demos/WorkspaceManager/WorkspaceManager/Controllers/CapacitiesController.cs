using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkspaceManager.Models;

namespace WorkspaceManager.Controllers {

  [Authorize]
  public class CapacitiesController : Controller {

    public async Task<ActionResult> Index() {
      var capacities = await PowerBiServiceApiManager.GetCapacitiesAsync();
      return View(capacities);
    }

  }
}