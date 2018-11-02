using System;
using System.IO;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

class Program {
  static string aadAuthorizationEndpoint = "https://login.windows.net/common/oauth2/authorize";
  static string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
  static string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

  public const string clientId = "315e87eb-a6a0-4886-9b20-9f7ecdaca888";
  public const string redirectUrl = "https://localhost/app1234";

  static string GetAccessToken() {

    // create new authentication context 
    var authenticationContext =
      new AuthenticationContext(aadAuthorizationEndpoint);

    //use authentication context to trigger user sign -in and return access token
    var userAuthnResult =
      authenticationContext.AcquireTokenAsync(resourceUriPowerBi,
                                              clientId,
                                              new Uri(redirectUrl),
                                              new PlatformParameters(PromptBehavior.Auto)).Result;

    //var userAuthnResult =
    // authenticationContext.AcquireTokenAsync(resourceUriPowerBi,
    //                                         clientId,
    //                                         new UserPasswordCredential("","")).Result;



    // return access token to caller
    return userAuthnResult.AccessToken;

  }

  static PowerBIClient GetPowerBiClient() {
    var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
    return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
  }

  static void Main() {
    CloneAppWorkspace("WorkspaceTemplate", "Customer C");

  }

  static void CreateAppWorkspace(string Name) {

    PowerBIClient pbiClient = GetPowerBiClient();

    GroupCreationRequest request = new GroupCreationRequest(Name);
    Group aws = pbiClient.Groups.CreateGroup(request);

    GroupUserAccessRight user1Permissions = new GroupUserAccessRight("Admin", "pbiemasteruser@sharepointconfessions.onmicrosoft.com");
    pbiClient.Groups.AddGroupUser(aws.Id, user1Permissions);

    string customersCapcityId = "C9CCAA3E-18FB-4F2E-930F-CD3ABF609B8A";
    AssignToCapacityRequest capReq = new AssignToCapacityRequest(customersCapcityId);
    pbiClient.Groups.AssignToCapacity(aws.Id, capReq);

  }

  static void CloneAppWorkspace(string sourceAppWorkspaceName, string targetAppWorkpaceName) {

    PowerBIClient pbiClient = GetPowerBiClient();
    string sourceAppWorkspaceId = "";
    string targetAppWorkspaceId = "";

    var workspaces = pbiClient.Groups.GetGroups().Value;
    foreach (var workspace in workspaces) {
      if (workspace.Name.Equals(sourceAppWorkspaceName)) {
        sourceAppWorkspaceId = workspace.Id;
      }
      if (workspace.Name.Equals(targetAppWorkpaceName)) {
        targetAppWorkspaceId = workspace.Id;
      }
    }

    if (sourceAppWorkspaceId == "") {
      throw new ApplicationException("Source Workspace does not exist");
    }

    if (targetAppWorkspaceId == "") {
      // create app workspace if it doesn't exist
      Console.WriteLine("Creating app workspace named " + targetAppWorkpaceName);
      GroupCreationRequest request = new GroupCreationRequest(targetAppWorkpaceName);
      Group AppWorkspace = pbiClient.Groups.CreateGroup(request);
      targetAppWorkspaceId = AppWorkspace.Id;

      string masterUserAccount = "pbiemasteruser@sharepointconfessions.onmicrosoft.com";
      Console.WriteLine("Configuring " + masterUserAccount + " as workspace admin");
      GroupUserAccessRight user1Permissions = new GroupUserAccessRight("Admin", masterUserAccount);
      pbiClient.Groups.AddGroupUser(targetAppWorkspaceId, user1Permissions);

      Console.WriteLine("Configuring workspace to run in dedicated capacity");
      string customersCapcityId = "C9CCAA3E-18FB-4F2E-930F-CD3ABF609B8A";
      AssignToCapacityRequest capReq = new AssignToCapacityRequest(customersCapcityId);
      pbiClient.Groups.AssignToCapacity(targetAppWorkspaceId, capReq);

      Console.WriteLine();

    }

    var reports = pbiClient.Reports.GetReportsInGroup(sourceAppWorkspaceId).Value;

    foreach (var report in reports) {
      var reportStream = pbiClient.Reports.ExportReportInGroup(sourceAppWorkspaceId, report.Id);
      string filePath = @"C:\tempExport\" + report.Name + ".pbix";
      Console.WriteLine("Downloading PBIX file for " + report.Name + "to " + filePath);
      FileStream stream1 = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
      reportStream.CopyToAsync(stream1).Wait();
      reportStream.Close();
      stream1.Close();
      stream1.Dispose();

      FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
      Console.WriteLine("Publishing " + filePath + " to " + targetAppWorkpaceName);
      var import = pbiClient.Imports.PostImportWithFileInGroup(targetAppWorkspaceId, stream, report.Name);

      Console.WriteLine("Deleing file " + filePath);
      stream.Close();
      stream.Dispose();
      File.Delete(filePath);

      Console.WriteLine();
    }

    Console.WriteLine("Export/Import process completed");

    var dashboards = pbiClient.Dashboards.GetDashboardsInGroup(sourceAppWorkspaceId).Value;

    foreach (var sourceDashboard in dashboards) {
      // create the target dashboard
      Console.WriteLine();
      Console.WriteLine("Creating Dashboard named " + sourceDashboard.DisplayName);
      AddDashboardRequest addReq = new AddDashboardRequest(sourceDashboard.DisplayName);
      Dashboard targetDashboard = pbiClient.Dashboards.AddDashboardInGroup(targetAppWorkspaceId, addReq);

      // clone tiles
      IList<Tile> sourceTiles = pbiClient.Dashboards.GetTilesInGroup(sourceAppWorkspaceId, sourceDashboard.Id).Value;
      foreach (Tile sourceTile in sourceTiles) {
        Console.WriteLine("Adding title with title of " + sourceTile.Title);
        var sourceDatasetID = sourceTile.DatasetId;
        var sourceDatasetName = pbiClient.Datasets.GetDatasetByIdInGroup(sourceAppWorkspaceId, sourceDatasetID).Name;
        var targetWorkspaceDatasets = pbiClient.Datasets.GetDatasetsInGroup(targetAppWorkspaceId).Value;
        string targetDatasetId = "";
        foreach (var ds in targetWorkspaceDatasets) {
          if (ds.Name.Equals(sourceDatasetName)) {
            targetDatasetId = ds.Id;
          }
        }
        if (targetDatasetId.Equals("")) throw new ApplicationException("OOOOOuch!");

        var sourceReportId = sourceTile.ReportId;
        var sourceReportName = pbiClient.Reports.GetReportInGroup(sourceAppWorkspaceId, sourceReportId).Name;

        var targetWorkspaceReports = pbiClient.Reports.GetReportsInGroup(targetAppWorkspaceId).Value;
        string targetReportId = "";
        foreach (var r in targetWorkspaceReports) {
          if (r.Name.Equals(sourceReportName)) {
            targetReportId = r.Id;
          }
        }

        CloneTileRequest addReqTile = new CloneTileRequest(targetDashboard.Id, targetAppWorkspaceId, targetReportId, targetDatasetId);
        pbiClient.Dashboards.CloneTileInGroup(sourceAppWorkspaceId, sourceDashboard.Id, sourceTile.Id, addReqTile);

      }

     

    }

    Console.WriteLine();
    Console.WriteLine("All done - wow that was a lot of work :)");
    Console.WriteLine();

  }

  static void PublishPBIX(string PbixFilePath, string ImportName) {
    Console.WriteLine("Publishing " + PbixFilePath);
    PowerBIClient pbiClient = GetPowerBiClient();
    FileStream stream = new FileStream(PbixFilePath, FileMode.Open, FileAccess.Read);
    var import = pbiClient.Imports.PostImportWithFile(stream, ImportName);
    Console.WriteLine("Publishing process completed");

  }

  static void RefreshDataset(string DatasetName) {
    PowerBIClient pbiClient = GetPowerBiClient();
    IList<Dataset> datasets = pbiClient.Datasets.GetDatasets().Value;
    foreach (var dataset in datasets) {
      if (dataset.Name.Equals(DatasetName)) {
        pbiClient.Datasets.RefreshDataset(dataset.Id);

      }
    }
  }

  public static void PatchDatasourceCredentials(string importName, string sqlAzureUser, string sqlAzurePassword) {

    PowerBIClient pbiClient = GetPowerBiClient();
    IList<Dataset> datasets = pbiClient.Datasets.GetDatasets().Value;
    foreach (var dataset in datasets) {
      if (dataset.Name.Equals(importName)) {

        var Datasource = pbiClient.Datasets.GetGatewayDatasources(dataset.Id).Value[0];
        string DatasourceId = Datasource.Id;
        string GatewayId = Datasource.GatewayId;

        // patching credentials does not yet work through the v2 API
        // you must complete this action with a direct HTTP PATCH request

        // create URL with pattern v1.0/myorg/gateways/{gateway_id}/datasources/{datasource_id}
        string restUrlPatchCredentials =
          urlPowerBiRestApiRoot + "" +
          "v1.0/myorg/" +
          "gateways/" + GatewayId + "/" +
          "datasources/" + DatasourceId + "/";

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
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken());

        // send PATCH request to Power BI service 
        var result = client.SendAsync(request).Result;

        Console.WriteLine("Credentials have been updated..");
        Console.WriteLine();

      }
    }

  }

}

public class DataSourceCredentials {
  public string credentialType { get; set; }
  public BasicCredentials basicCredentials { get; set; }
}

public class BasicCredentials {
  public string username { get; set; }
  public string password { get; set; }
}
