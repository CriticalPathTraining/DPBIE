using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;

class Program {

  static string aadAuthorizationEndpoint = "https://login.windows.net/common";
  static string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
  static string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

  // enter the correct configuration values for your environment
  static string appWorkspaceId = "";
  static string clientId = "";
  static string redirectUrl = "https://localhost/app1234";

  static string GetAccessToken() {

    // create new authentication context 
    var authenticationContext = new AuthenticationContext(aadAuthorizationEndpoint);

    // use authentication context to sign-in using User Password Credentials flow
    string masterUserAccount = "";
    string masterUserPassword = "";
    UserPasswordCredential creds = new UserPasswordCredential(masterUserAccount, masterUserPassword);

    var userAuthnResult =
      authenticationContext.AcquireTokenAsync(resourceUriPowerBi, clientId, creds).Result;

    // return access token to caller
    return userAuthnResult.AccessToken;

  }


  static PowerBIClient GetPowerBiClient() {
    var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
    return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
  }

  static void Main() {

    DisplayPersonalWorkspaceAssets();

    CreateAppWorkspace("AWS 1");

    string appWorkspaceId = CreateAppWorkspace("AWS 2");
    string pbixPath = @"C:\Student\PBIX\Wingtip Sales Analysis.pbix";
    string importName = "Wingtip Sales";
    PublishPBIX(appWorkspaceId, pbixPath, importName);

    CloneAppWorkspace("Wingtip Sales", "AWS 3");

  }

  static void DisplayPersonalWorkspaceAssets() {

    PowerBIClient pbiClient = GetPowerBiClient();

    Console.WriteLine("Listing assets in app workspace: " + appWorkspaceId);
    Console.WriteLine();

    Console.WriteLine("Datasets:");
    var datasets = pbiClient.Datasets.GetDatasetsInGroup(appWorkspaceId).Value;
    foreach (var dataset in datasets) {
      Console.WriteLine(" - " + dataset.Name + " [" + dataset.Id + "]");
    }

    Console.WriteLine();
    Console.WriteLine("Reports:");
    var reports = pbiClient.Reports.GetReportsInGroup(appWorkspaceId).Value;
    foreach (var report in reports) {
      Console.WriteLine(" - " + report.Name + " [" + report.Id + "]");
    }

    Console.WriteLine();
    Console.WriteLine("Dashboards:");
    var dashboards = pbiClient.Dashboards.GetDashboardsInGroup(appWorkspaceId).Value;
    foreach (var dashboard in dashboards) {
      Console.WriteLine(" - " + dashboard.DisplayName + " [" + dashboard.Id + "]");
    }

    Console.WriteLine();
  }

  static string CreateAppWorkspace(string Name) {
    PowerBIClient pbiClient = GetPowerBiClient();
    // create new app workspace
    GroupCreationRequest request = new GroupCreationRequest(Name);
    Group aws = pbiClient.Groups.CreateGroup(request);
    // return app workspace ID
    return aws.Id;
  }

  static void PublishPBIX(string appWorkspaceId, string PbixFilePath, string ImportName) {
    Console.WriteLine("Publishing " + PbixFilePath);
    PowerBIClient pbiClient = GetPowerBiClient();
    FileStream stream = new FileStream(PbixFilePath, FileMode.Open, FileAccess.Read);
    var import = pbiClient.Imports.PostImportWithFileInGroup(appWorkspaceId, stream, ImportName);
    Console.WriteLine("Publishing process completed");
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
      Console.WriteLine();
      GroupCreationRequest request = new GroupCreationRequest(targetAppWorkpaceName);
      Group AppWorkspace = pbiClient.Groups.CreateGroup(request);
      targetAppWorkspaceId = AppWorkspace.Id;      
    }

    var reports = pbiClient.Reports.GetReportsInGroup(sourceAppWorkspaceId).Value;

    string downloadPath = @"C:\Student\downloads\";
    // create download folder if it doesn't exist
    if (!Directory.Exists(downloadPath)) {
      Directory.CreateDirectory(downloadPath);
    }

    foreach (var report in reports) {
      var reportStream = pbiClient.Reports.ExportReportInGroup(sourceAppWorkspaceId, report.Id);
      string filePath = downloadPath + report.Name + ".pbix";
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
        Console.WriteLine("Adding dashboard tile with title of " + sourceTile.Title);
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


}

