using HelloWorldUsingRest.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldUsingRest {
  class Program {

    public const string AzureAuthorizationEndpoint = "https://login.microsoftonline.com/common";
    public const string PowerBiServiceResourceUri = "https://analysis.windows.net/powerbi/api";
    public const string PowerBiServiceRootUrl = "https://api.powerbi.com/v1.0/myorg/";

    // Commonly-used Power BI REST URLs
    public const string restUrlWorkspaces = "https://api.powerbi.com/v1.0/myorg/groups/";
    public const string restUrlDatasets = "https://api.powerbi.com/v1.0/myorg/datasets/";
    public const string restUrlReports = "https://api.powerbi.com/v1.0/myorg/reports/";


    public const string ClientID = "315e87eb-a6a0-4886-9b20-9f7ecdaca888";
    public const string RedirectUri = "https://localhost/app1234";

    protected static string AccessToken = string.Empty;

    static void GetAccessToken() {

      // create new authentication context 
      var authenticationContext = new AuthenticationContext(AzureAuthorizationEndpoint);

      //// use authentication context to trigger user sign-in and return access token 
      //var userAuthnResult = authenticationContext.AcquireTokenAsync(PowerBiServiceResourceUri,
      //                                                         ClientID,
      //                                                         new Uri(RedirectUri),
      //                                                         new PlatformParameters(PromptBehavior.Auto)).Result;

      // use authentication context to trigger user sign-in and return access token 

      string userPassword = System.Configuration.ConfigurationManager.AppSettings["userPassword"];
      string userName = "tedp@sharepointconfessions.onMicrosoft.com";

      UserPasswordCredential creds = new UserPasswordCredential(userName, userPassword);
      var userAuthnResult = authenticationContext.AcquireTokenAsync(PowerBiServiceResourceUri,
                                                               ClientID,
                                                               creds).Result;

      // cache access token in AccessToken field
      AccessToken = userAuthnResult.AccessToken;

    }

    private static string ExecuteGetRequest(string restUri) {

      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
      client.DefaultRequestHeaders.Add("Accept", "application/json");

      HttpResponseMessage response = client.GetAsync(restUri).Result;

      if (response.IsSuccessStatusCode) {
        return response.Content.ReadAsStringAsync().Result;
      }
      else {
        Console.WriteLine();
        Console.WriteLine("OUCH! - error occurred during GET REST call");
        Console.WriteLine();
        return string.Empty;
      }
    }


    static void Main() {
      GetAccessToken();

      DisplayWorkspaceContents();
    }

    public static void DisplayWorkspaceContents() {

      string jsonWorkspaces = ExecuteGetRequest(restUrlWorkspaces);
      WorkspaceCollection workspaces = JsonConvert.DeserializeObject<WorkspaceCollection>(jsonWorkspaces);
      Console.WriteLine("Group Workspaces:");
      Console.WriteLine("-----------------");
      foreach (Workspace workspace in workspaces.value) {
        Console.WriteLine(" - " + workspace.name + "(" + workspace.id + ")");
      }
      Console.WriteLine();
      Console.WriteLine("Now examining content in your personal workspace...");
      Console.WriteLine();

      string jsonDatasets = ExecuteGetRequest(restUrlDatasets);
      DatasetCollection datasets = JsonConvert.DeserializeObject<DatasetCollection>(jsonDatasets);
      Console.WriteLine("Datasets:");
      Console.WriteLine("---------");
      foreach (var ds in datasets.value) {
        Console.WriteLine(" - " + ds.name + "(" + ds.id + ")");
      }
      Console.WriteLine();

      string jsonReports = ExecuteGetRequest(restUrlReports);
      ReportCollection reports = JsonConvert.DeserializeObject<ReportCollection>(jsonReports);
      Console.WriteLine("Reports:");
      Console.WriteLine("---------");
      foreach (var report in reports.value) {
        Console.WriteLine(" - " + report.name + ":   " + report.embedUrl);
      }
      Console.WriteLine();

      
    }

  }
}
