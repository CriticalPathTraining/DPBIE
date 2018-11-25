using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace HelloPowerBiServiceApi {

  class Program {

    const string aadAuthorizationEndpoint = "https://login.windows.net/common";
    const string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";

    static readonly Uri redirectUri = new Uri("https://localhost/app1234");

    const string clientId = "PASTE_YOUR_AZURE_APPLICATION_ID_HERE";
    const string appWorkspaceId = "PASTE_YOUR_POWER_BI_APP_WORKSPACE_ID_HERE";

    static string GetAccessToken() {
      var authContext = new AuthenticationContext(aadAuthorizationEndpoint);
      var promptBehavior = new PlatformParameters(PromptBehavior.RefreshSession);
      AuthenticationResult result =
        authContext.AcquireTokenAsync(resourceUriPowerBi, clientId, redirectUri, promptBehavior).Result;
      return result.AccessToken;
    }

    static string GetAccessTokenUnattended() {
      string userName = "REPLACE_WITH_MASTER_USER_ACCOUNT_NAME";
      string userPassword = "REPLACE_WITH_MASTER_USER_ACCOUNT_PASSWORD";
      var authContext = new AuthenticationContext(aadAuthorizationEndpoint);
      var userPasswordCredential = new UserPasswordCredential(userName, userPassword);
      AuthenticationResult result =
        authContext.AcquireTokenAsync(resourceUriPowerBi, clientId, userPasswordCredential).Result;
      return result.AccessToken;
    }

    static string ExecuteGetRequest(string restUrl) {
      HttpClient client = new HttpClient();
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, restUrl);
      request.Headers.Add("Authorization", "Bearer " + GetAccessToken());
      request.Headers.Add("Accept", "application/json;odata.metadata=minimal");
      HttpResponseMessage response = client.SendAsync(request).Result;
      if (response.StatusCode != HttpStatusCode.OK) {
        throw new ApplicationException("Error occured calling the Power BI Servide API");
      }
      return response.Content.ReadAsStringAsync().Result;
    }

    static void Main() {
      // get report data from app workspace
      string restUrl = "https://api.powerbi.com/v1.0/myorg/groups/" + appWorkspaceId + "/reports/";
      var json = ExecuteGetRequest(restUrl);
      ReportCollection reports = JsonConvert.DeserializeObject<ReportCollection>(json);
      foreach (Report report in reports.value) {
        Console.WriteLine("Report Name: " + report.name);
        Console.WriteLine("Report ID: " + report.id);
        Console.WriteLine("EmbedUrl: " + report.embedUrl);
        Console.WriteLine();
      }

      Console.Write("Press ENTER to continue...");
      Console.ReadLine();
    }

  }

  public class Report {
    public string id { get; set; }
    public string name { get; set; }
    public string webUrl { get; set; }
    public string embedUrl { get; set; }
    public bool isOwnedByMe { get; set; }
    public string datasetId { get; set; }
  }

  public class ReportCollection {
    public List<Report> value { get; set; }
  }

}