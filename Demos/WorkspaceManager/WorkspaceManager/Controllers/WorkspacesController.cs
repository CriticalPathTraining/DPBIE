using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkspaceManager.Models;

namespace WorkspaceManager.Controllers {

  public class NewWorkspaceData {
    public string Name { get; set; }
  }


  [Authorize]
  public class WorkspacesController : Controller {

    public async Task<ActionResult> Index() {
      var workspaces = await PowerBiServiceApiManager.GetWorkspacesAsync();
      return View(workspaces);
    }

    public ActionResult Create() {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create([Bind(Include = "Name")] NewWorkspaceData workspace) {

      await PowerBiServiceApiManager.CreateWorkspacesAsync(workspace.Name);

      return RedirectToAction("Index");
    }



  }

}