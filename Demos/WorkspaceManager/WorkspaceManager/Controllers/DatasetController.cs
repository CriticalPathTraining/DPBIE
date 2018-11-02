using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkspaceManager.Models;

namespace WorkspaceManager.Controllers {

  [Authorize]
  public class DatasetController : Controller {

    public async Task<ActionResult> Index(string workspaceid, string datasetid) {
      if (string.IsNullOrEmpty(workspaceid) || string.IsNullOrEmpty(datasetid)) {
        return RedirectToAction("Index", "Workspaces");
      }
      DatasetViewModel viewModel = await PowerBiServiceApiManager.GetDatasetAsync(workspaceid, datasetid);
      return View(viewModel);
    }

    public async Task<ActionResult> Refresh(string workspaceid, string datasetid) {
      if (string.IsNullOrEmpty(workspaceid) || string.IsNullOrEmpty(datasetid)) {
        return RedirectToAction("Index", "Workspaces");
      }
      await PowerBiServiceApiManager.RefreshDatasetAsync(workspaceid, datasetid);
      return RedirectToAction("Index", "Dataset", new { workspaceid=workspaceid, datasetid=datasetid });
    }
  }
}