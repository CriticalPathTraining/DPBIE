using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using DailyReporterPersonal.Models;

namespace DailyReporterPersonal.Controllers {

  [Authorize]
  public class ReportsController : Controller {

    public async Task<ActionResult> Index() {
      return View(await PowerBiEmbeddingManager.GetReports());
    }



  }
}