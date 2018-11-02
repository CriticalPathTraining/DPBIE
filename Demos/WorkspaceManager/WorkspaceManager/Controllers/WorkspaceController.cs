using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkspaceManager.Models;

namespace WorkspaceManager.Controllers {

  [Authorize]
  public class WorkspaceController : Controller {

    public async Task<ActionResult> Index(string workspaceid) {
      if (string.IsNullOrEmpty(workspaceid)) {
        return RedirectToAction("Index", "Workspaces");
      }
      WorkspaceViewModel viewModel = await PowerBiServiceApiManager.GetWorkspaceAsync(workspaceid);
      return View(viewModel);
    }

    public async Task<ActionResult> UploadPBIX(string workspaceId, string pbixName, string importName) {
      if (string.IsNullOrEmpty(workspaceId)) {
        throw new ApplicationException("Error: No target workspace ID!");
        //return RedirectToAction("Index", "Workspaces");
      }

      await PowerBiServiceApiManager.UploadPBIX(workspaceId, pbixName, importName);
      return RedirectToAction("Index", "Workspace", new { workspaceId = workspaceId });
    }

    public async Task<ActionResult> UploadPBIXandSetSqlCreds(string workspaceId, string pbixName, string importName) {
      if (string.IsNullOrEmpty(workspaceId)) {
        throw new ApplicationException("Error: No target workspace ID!");
        //return RedirectToAction("Index", "Workspaces");
      }

      await PowerBiServiceApiManager.UploadPBIX(workspaceId, pbixName, importName, true);


      return RedirectToAction("Index", "Workspace", new { workspaceId = workspaceId });
    }

    public async Task<ActionResult> DeleteWorkspace(string workspaceId) {
      if (string.IsNullOrEmpty(workspaceId)) {
        throw new ApplicationException("Error: No target workspace ID!");
        //return RedirectToAction("Index", "Workspaces");
      }

      await PowerBiServiceApiManager.DeleteWorkspaceAsync(workspaceId);

      return RedirectToAction("Index", "Workspaces");
    }
  }
}