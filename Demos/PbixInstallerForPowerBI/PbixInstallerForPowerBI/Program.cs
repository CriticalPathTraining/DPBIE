using System;
using Microsoft.Rest;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;
using PbixInstallerForPowerBI.Model;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;

namespace PbixInstallerForPowerBI {

  class ProgramConstants {

    // update client id to reference an application registred with Azure
    public const string ClientID = "[[YOUR_CLIENT_ID_HERE]]";

    // Redirect URL needs to match reply URL in Azure registration
    public const string RedirectUri = "https://localhost/PbixInstallerForPowerBI";

    // URLs for working with the Power BI REST API
    public const string AzureAuthorizationEndpoint = "https://login.microsoftonline.com/common";
    public const string PowerBiServiceResourceUri = "https://analysis.windows.net/powerbi/api";
    public const string PowerBiServiceRootUrl = "https://api.powerbi.com/v1.0/myorg/";

    // Commonly-used Power BI REST URLs
    public const string restUrlWorkspaces = "https://api.powerbi.com/v1.0/myorg/groups/";
    public const string restUrlDatasets = "https://api.powerbi.com/v1.0/myorg/datasets/";
    public const string restUrlReports = "https://api.powerbi.com/v1.0/myorg/reports/";
    public const string restUrlImports = "https://api.powerbi.com/v1.0/myorg/imports/";

    // login credentials for Azure SQL database
    public const string AzureSqlDatabaseLogin = "CptStudent";
    public const string AzureSqlDatabasePassword = "pass@word1";

  }

  class Program {

    protected static string AccessToken = string.Empty;

    protected static void AcquireAccessToken() {

      Console.WriteLine("Calling to acquire access token...");

      // create new ADAL authentication context 
      var authenticationContext =
        new AuthenticationContext(ProgramConstants.AzureAuthorizationEndpoint);

      //use authentication context to trigger user login and then acquire access token
      var userAuthnResult =
        authenticationContext.AcquireTokenAsync(ProgramConstants.PowerBiServiceResourceUri,
                                                ProgramConstants.ClientID,
                                                new Uri(ProgramConstants.RedirectUri),
                                                new PlatformParameters(PromptBehavior.Auto)).Result;


      //// use authentication context to trigger user sign-in and return access token 
      //var userCreds = new UserPasswordCredential("UserName@MyTenent.onMicrosoft.com", "myEasyToCrackPassword");

      //var userAuthnResult = authenticationContext.AcquireTokenAsync(ProgramConstants.PowerBiServiceResourceUri,
      //                                                              ProgramConstants.ClientID,
      //                                                              userCreds).Result;


      // cache access token in AccessToken field
      AccessToken = userAuthnResult.AccessToken;

      Console.WriteLine(" - access token successfully acquired");
      Console.WriteLine();
    }

    #region "REST operation utility methods"

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

    private static string ExecutePostRequest(string restUri, string postBody) {

      try {
        HttpContent body = new StringContent(postBody);
        body.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
        HttpResponseMessage response = client.PostAsync(restUri, body).Result;

        if (response.IsSuccessStatusCode) {
          return response.Content.ReadAsStringAsync().Result;
        }
        else {
          Console.WriteLine();
          Console.WriteLine("OUCH! - error occurred during POST REST call");
          Console.WriteLine();
          return string.Empty;
        }
      }
      catch {
        Console.WriteLine();
        Console.WriteLine("OUCH! - error occurred during POST REST call");
        Console.WriteLine();
        return string.Empty;
      }
    }

    private static string ExecuteDeleteRequest(string restUri) {
      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Add("Accept", "application/json");
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
      HttpResponseMessage response = client.DeleteAsync(restUri).Result;

      if (response.IsSuccessStatusCode) {
        return response.Content.ReadAsStringAsync().Result;
      }
      else {
        Console.WriteLine();
        Console.WriteLine("OUCH! - error occurred during Delete REST call");
        Console.WriteLine();
        return string.Empty;
      }
    }

    #endregion

    public static void DisplayWorkspaceContents() {

      string jsonWorkspaces = ExecuteGetRequest(ProgramConstants.restUrlWorkspaces);
      WorkspaceCollection workspaces = JsonConvert.DeserializeObject<WorkspaceCollection>(jsonWorkspaces);
      Console.WriteLine("Group Workspaces:");
      Console.WriteLine("-----------------");
      foreach (Workspace workspace in workspaces.value) {
        Console.WriteLine(" - " + workspace.name + "(" + workspace.id + ")");
      }
      Console.WriteLine();
      Console.WriteLine("Now examining content in your personal workspace...");
      Console.WriteLine();

      string jsonDatasets = ExecuteGetRequest(ProgramConstants.restUrlDatasets);
      DatasetCollection datasets = JsonConvert.DeserializeObject<DatasetCollection>(jsonDatasets);
      Console.WriteLine("Datasets:");
      Console.WriteLine("---------");
      foreach (var ds in datasets.value) {
        Console.WriteLine(" - " + ds.name + "(" + ds.id + ")");
      }
      Console.WriteLine();

      string jsonReports = ExecuteGetRequest(ProgramConstants.restUrlReports);
      ReportCollection reports = JsonConvert.DeserializeObject<ReportCollection>(jsonReports);
      Console.WriteLine("Reports:");
      Console.WriteLine("---------");
      foreach (var report in reports.value) {
        Console.WriteLine(" - " + report.name + ":   " + report.embedUrl);
      }
      Console.WriteLine();

      string jsonImports = ExecuteGetRequest(ProgramConstants.restUrlImports);
      ImportCollection imports = JsonConvert.DeserializeObject<ImportCollection>(jsonImports);
      Console.WriteLine("Imports:");
      Console.WriteLine("---------");
      foreach (var import in imports.value) {
        Console.WriteLine(" - " + import.name + ":   " + import.source);
      }
      Console.WriteLine();

    }

    public static void DeleteImport(string importName) {
      // check to see if import already exists by inspecting dataset names
      string restUrlDatasets = ProgramConstants.PowerBiServiceRootUrl + "datasets/";
      string jsonDatasets = ExecuteGetRequest(restUrlDatasets);
      DatasetCollection datasets = JsonConvert.DeserializeObject<DatasetCollection>(jsonDatasets);
      foreach (var dataset in datasets.value) {
        if (importName.Equals(dataset.name)) {
          // if dataset name matches, delete dataset which will effective delete the entire import
          Console.WriteLine("Deleting existing import named " + dataset.name);
          string restUrlDatasetToDelete = ProgramConstants.PowerBiServiceRootUrl + "datasets/" + dataset.id;
          ExecuteDeleteRequest(restUrlDatasetToDelete);
        }
      }
    }

    public static void ImportPBIX(string pbixFilePath, string importName) {
      // delete exisitng import of the same name if on exists
      DeleteImport(importName);
      // create REST URL with import name in quer string
      string restUrlImportPbix = ProgramConstants.PowerBiServiceRootUrl + "imports?datasetDisplayName=" + importName;
      // load PBIX file into StreamContent object
      var pbixBodyContent = new StreamContent(File.Open(pbixFilePath, FileMode.Open));
      // add headers for request bod content
      pbixBodyContent.Headers.Add("Content-Type", "application/octet-stream");
      pbixBodyContent.Headers.Add("Content-Disposition",
                                   @"form-data; name=""file""; filename=""" + pbixFilePath + @"""");
      // load PBIX content into body using multi-part form data
      MultipartFormDataContent requestBody = new MultipartFormDataContent(Guid.NewGuid().ToString());
      requestBody.Add(pbixBodyContent);
      // create and configure HttpClient
      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Add("Accept", "application/json");
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
      // post request
      var response = client.PostAsync(restUrlImportPbix, requestBody).Result;
      // check for success
      if (response.StatusCode.ToString().Equals("Accepted")) {
        Console.WriteLine("Import process complete: " + response.Content.ReadAsStringAsync().Result);
      }
    }

    public static void PatchDatasourceCredentials(string importName, string sqlAzureUser, string sqlAzurePassword) {

      string restUrlDatasets = ProgramConstants.PowerBiServiceRootUrl + "datasets/";
      string jsonDatasets = ExecuteGetRequest(restUrlDatasets);

      DatasetCollection datasets = JsonConvert.DeserializeObject<DatasetCollection>(jsonDatasets);
      foreach (var dataset in datasets.value) {
        // find dataset whose name matches import name
        if (importName.Equals(dataset.name)) {
          Console.WriteLine("Updating data source for dataset named " + dataset.name);
          // determine gateway id and datasoure id
          string restUrlDatasetToUpdate = ProgramConstants.restUrlDatasets + dataset.id + "/";
          string restUrlDatasetDefaultGateway = restUrlDatasetToUpdate + "Default.GetBoundGatewayDataSources";
          string jsonDefaultGateway = ExecuteGetRequest(restUrlDatasetDefaultGateway);

          Gateway defaultGateway = (JsonConvert.DeserializeObject<GatewayCollection>(jsonDefaultGateway)).value[0];
          Console.WriteLine(" - Gateway ID: " + defaultGateway.gatewayId);
          Console.WriteLine(" - Datasource ID: " + defaultGateway.id);

          // create URL with pattern myorg/gateways/{gateway_id}/datasources/{datasource_id}
          string restUrlPatchCredentials =
            ProgramConstants.PowerBiServiceRootUrl +
            "gateways/" + defaultGateway.gatewayId + "/" +
            "datasources/" + defaultGateway.id + "/";

          // create C# object with credential data
          DataSourceCredentials dataSourceCredentials =
            new DataSourceCredentials {
              credentialType = "Basic",
              basicCredentials = new BasicCredentials {
                username = sqlAzureUser,
                password = sqlAzurePassword
              }
            };

          // serialize C# object into JSON
          string jsonDelta = JsonConvert.SerializeObject(dataSourceCredentials);

          // add JSON to HttpContent object and configure content type
          HttpContent patchRequestBody = new StringContent(jsonDelta);
          patchRequestBody.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

          // prepare PATCH request
          var method = new HttpMethod("PATCH");
          var request = new HttpRequestMessage(method, restUrlPatchCredentials);
          request.Content = patchRequestBody;

          HttpClient client = new HttpClient();
          client.DefaultRequestHeaders.Add("Accept", "application/json");
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

          client.SendAsync(request);

          Console.WriteLine("Credentials have been updated..");
          Console.WriteLine();
        }
      }
    }



    static void Main() {

      AcquireAccessToken();

      //DisplayWorkspaceContents();

      string pbixFilePath = @"C:\DevProjects\Git\PBIX\WingtipSalesDirectQuery.pbix";
      string importName = "Wingtip Sales Direct Query";
      
      ImportPBIX(pbixFilePath, importName);      
      PatchDatasourceCredentials(importName, "CptStudent", "pass@word1");

    }



  }
}
