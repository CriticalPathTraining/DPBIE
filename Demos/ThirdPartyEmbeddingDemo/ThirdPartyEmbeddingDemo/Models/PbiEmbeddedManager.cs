using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;

namespace ThirdPartyEmbeddingDemo.Models {

  public class PbiEmbeddedManager {

    private static string aadAuthorizationEndpoint = "https://login.microsoftonline.com/common";
    private static string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
    private static string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

    private static string applicationId = ConfigurationManager.AppSettings["application-id"];

    private static string workspaceId = ConfigurationManager.AppSettings["app-workspace-id"];
    private static string datasetId = ConfigurationManager.AppSettings["dataset-id"];
    private static string reportId = ConfigurationManager.AppSettings["report-id"];
    private static string dashboardId = ConfigurationManager.AppSettings["dashboard-id"];

    private static string userName = ConfigurationManager.AppSettings["aad-account-name"];
    private static string userPassword = ConfigurationManager.AppSettings["aad-account-password"];

    private static string GetAccessToken() {

      AuthenticationContext authenticationContext = new AuthenticationContext(aadAuthorizationEndpoint);

      AuthenticationResult userAuthnResult =
        authenticationContext.AcquireTokenAsync(
          resourceUriPowerBi,
          applicationId,
          new UserPasswordCredential(userName, userPassword)).Result;

      return userAuthnResult.AccessToken;
    }

    private static PowerBIClient GetPowerBiClient() {
      var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
      return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
    }

    public static async Task<ReportEmbeddingData> GetReportEmbeddingData() {

      string currentUserName = HttpContext.Current.User.Identity.GetUserName();
      ApplicationDbContext context = new ApplicationDbContext();
      var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
      ApplicationUser currentUser = userManager.FindByName(currentUserName);

      var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

      List<string> roles = new List<string>();

      foreach (var role in currentUser.Roles) {
        roles.Add(roleManager.FindById(role.RoleId).Name);
      }

      string accessLevel = HttpContext.Current.User.IsInRole("Admin") ? "edit" : "view";

      PowerBIClient pbiClient = GetPowerBiClient();

      var report = await pbiClient.Reports.GetReportInGroupAsync(workspaceId, reportId);
      var embedUrl = report.EmbedUrl;
      var reportName = report.Name;

      GenerateTokenRequest generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: accessLevel);
      string embedToken =
            (await pbiClient.Reports.GenerateTokenInGroupAsync(workspaceId,
                                                               report.Id,
                                                               generateTokenRequestParameters)).Token;

      return new ReportEmbeddingData {
        reportId = reportId,
        reportName = reportName,
        embedUrl = embedUrl,
        accessToken = embedToken
      };

    }

  }

}