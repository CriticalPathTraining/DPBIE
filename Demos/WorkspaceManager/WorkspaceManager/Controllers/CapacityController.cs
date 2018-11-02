using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkspaceManager.Models;

namespace WorkspaceManager.Controllers {

  [Authorize]
  public class CapacityController : Controller {

    public async Task<ActionResult> Index(string capacityid) {
      if (string.IsNullOrEmpty(capacityid)) {
        return RedirectToAction("Index", "Capacities");
      }
      CapacityViewModel viewModel = await PowerBiServiceApiManager.GetCapacityAsync(capacityid);
      return View(viewModel);
    }

  }
}