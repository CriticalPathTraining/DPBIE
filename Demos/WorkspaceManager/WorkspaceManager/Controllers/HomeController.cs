using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkspaceManager.Models;

namespace WorkspaceManager.Controllers {

  public class HomeController : Controller {

    public ActionResult Index() {
      return View();
    }

    public ActionResult About() {
      ViewBag.Title = "About this Demo Web Application";
      ViewBag.Message = "A demo by Ted Pattison for exploring the Power BI Service API.";
      ViewBag.PBI_API_VERSION = PowerBiServiceApiManager.GetPowerBiAssemblyInfo();
      return View();
    }

  }
}