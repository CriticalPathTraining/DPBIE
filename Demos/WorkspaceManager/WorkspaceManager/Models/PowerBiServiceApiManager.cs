using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace WorkspaceManager.Models {

  public class PowerBiServiceApiManager {

    #region "private implementation details"

    private static string aadInstance = "https://login.microsoftonline.com/";
    private static string resourceUrlPowerBi = "https://analysis.windows.net/powerbi/api";
    private static string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

    private static string clientId = ConfigurationManager.AppSettings["client-id"];
    private static string clientSecret = ConfigurationManager.AppSettings["client-secret"];
    private static string redirectUrl = ConfigurationManager.AppSettings["reply-url"];

    private static async Task<string> GetAccessTokenAsync() {

      // determine authorization URL for current tenant
      string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
      string tenantAuthority = aadInstance + tenantID;

      // create ADAL cache object
      ApplicationDbContext db = new ApplicationDbContext();
      string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
      ADALTokenCache userTokenCache = new ADALTokenCache(signedInUserID);

      // create authentication context
      AuthenticationContext authenticationContext = new AuthenticationContext(tenantAuthority, userTokenCache);

      // create client credential object using client ID and client Secret"];
      ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);

      // create user identifier object for logged on user
      string objectIdentifierId = "http://schemas.microsoft.com/identity/claims/objectidentifier";
      string userObjectID = ClaimsPrincipal.Current.FindFirst(objectIdentifierId).Value;
      UserIdentifier userIdentifier = new UserIdentifier(userObjectID, UserIdentifierType.UniqueId);

      // get access token for Power BI Service API from AAD
      AuthenticationResult authenticationResult =
        await authenticationContext.AcquireTokenSilentAsync(
            resourceUrlPowerBi,
            clientCredential,
            userIdentifier);

      // return access token back to user
      return authenticationResult.AccessToken;

    }

    private static PowerBIClient GetPowerBiClient() {
      string accessToken = GetAccessTokenAsync().Result;
      TokenCredentials tokenCredentials = new TokenCredentials(accessToken, "Bearer");
      return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
    }

    #endregion

    public static string GetPowerBiAssemblyInfo() {
      return (new BasicCredentials()).GetType().Assembly.FullName;
    }

    public static async Task<IList<Group>> GetWorkspacesAsync() {
      PowerBIClient pbiClient = GetPowerBiClient();
      return (await pbiClient.Groups.GetGroupsAsAdminAsync(filter: "state eq 'Active'")).Value;
    }


    public static async Task<WorkspaceViewModel> GetWorkspaceAsync(string WorkspaceId) {

      PowerBIClient pbiClient = GetPowerBiClient();

      var workspaces = (await pbiClient.Groups.GetGroupsAsAdminAsync()).Value;
      var workspace = workspaces.Where(ws => ws.Id.Equals(WorkspaceId)).FirstOrDefault();
      var workspaceUsers = (await pbiClient.Groups.GetGroupUsersAsync(WorkspaceId)).Value;
      var datasets = (await pbiClient.Datasets.GetDatasetsInGroupAsAdminAsync(WorkspaceId)).Value;
      var reports = (await pbiClient.Reports.GetReportsInGroupAsAdminAsync(WorkspaceId)).Value;
      var dashboards = (await pbiClient.Dashboards.GetDashboardsInGroupAsAdminAsync(WorkspaceId)).Value;

      WorkspaceViewModel viewModel = new WorkspaceViewModel {
        Id = workspace.Id,
        Name = workspace.Name,
        Workspace = workspace,
        workspaceUsers = workspaceUsers,
        Datasets = datasets,
        Reports = reports,
        Dashboards = dashboards
      };

      return viewModel;
    }

    public static async Task<IList<Capacity>> GetCapacitiesAsync() {
      PowerBIClient pbiClient = GetPowerBiClient();
      return (await pbiClient.Capacities.GetCapacitiesAsync()).Value;
    }

    public static async Task<CapacityViewModel> GetCapacityAsync(string CapcityId) {

      PowerBIClient pbiClient = GetPowerBiClient();
      var capacities = (await pbiClient.Capacities.GetCapacitiesAsync()).Value;

      foreach (var capacity in capacities) {
        if (CapcityId.Equals(capacity.Id)) {

          CapacityViewModel viewModel = new CapacityViewModel {
            Id = capacity.Id,
            DisplayName = capacity.DisplayName
          };
          return viewModel;
        }
      }
      throw new ApplicationException("No capacity with that ID");

    }

    public static async Task<DatasetViewModel> GetDatasetAsync(string WorkspaceId, string DatasetId) {

      PowerBIClient pbiClient = GetPowerBiClient();
      IList<Dataset> datasets = (await pbiClient.Datasets.GetDatasetsInGroupAsAdminAsync(WorkspaceId)).Value;

      var dataset = datasets.Where(ds => ds.Id.Equals(DatasetId)).Single();

      IList<Datasource> datasources = (await pbiClient.Datasets.GetDatasourcesAsAdminAsync(DatasetId)).Value;
      IList<Refresh> refreshHistory = null;

      if (dataset.IsRefreshable == true) {
        refreshHistory = (await pbiClient.Datasets.GetRefreshHistoryInGroupAsync(WorkspaceId, DatasetId)).Value;
      }      
      
      DatasetViewModel viewModel = new DatasetViewModel {
        WorkspaceId=WorkspaceId,
        Id = dataset.Id,
        Name = dataset.Name,
        Dataset = dataset,
        Datasources = datasources,
        RefreshHistroy = refreshHistory
      };

      return viewModel;
    }

    public static async Task RefreshDatasetAsync(string WorkspaceId, string DatasetId) {
      PowerBIClient pbiClient = GetPowerBiClient();
      await pbiClient.Datasets.RefreshDatasetInGroupAsync(WorkspaceId, DatasetId);
      return;
    }

    public static async Task<Group> CreateWorkspacesAsync(string WorkspaceName) {

      PowerBIClient pbiClient = GetPowerBiClient();
      GroupCreationRequest createRequest = new GroupCreationRequest(WorkspaceName);
      var workspace = await pbiClient.Groups.CreateGroupAsync(createRequest);

      //var secondaryAdmin = "pbiemasteruser@sharepointconfessions.onmicrosoft.com";
      //var userRights = new GroupUserAccessRight("Admin", secondaryAdmin);
      //await pbiClient.Groups.AddGroupUserAsync(workspace.Id, userRights);

      return workspace;

    }

    public static async Task<bool> DeleteWorkspaceAsync(string WorkspaceId) {
      PowerBIClient pbiClient = GetPowerBiClient();
      await pbiClient.Groups.DeleteGroupAsync(WorkspaceId);
      return true;
    }

    public static async Task UploadPBIX(string WorkspaceId, string pbixName, string importName, bool updateSqlCredentials = false) {

      string PbixFilePath = HttpContext.Current.Server.MapPath("/PBIX/" + pbixName);
      PowerBIClient pbiClient = GetPowerBiClient();
      FileStream stream = new FileStream(PbixFilePath, FileMode.Open, FileAccess.Read);
      var import = await pbiClient.Imports.PostImportWithFileAsyncInGroup(WorkspaceId, stream, importName);

      if (updateSqlCredentials) {
        await PatchSqlDatasourceCredentials(WorkspaceId, importName);
      }

      return;
    }

    public static async Task PatchSqlDatasourceCredentials(string WorkspaceId, string importName) {
      PowerBIClient pbiClient = GetPowerBiClient();
      var datasets = (await pbiClient.Datasets.GetDatasetsInGroupAsync(WorkspaceId)).Value;
      foreach (var dataset in datasets) {
        if (importName.Equals(dataset.Name)) {
          string datasetId = dataset.Id;
          var datasources = (await pbiClient.Datasets.GetDatasourcesInGroupAsync(WorkspaceId, datasetId)).Value;
          foreach (var datasource in datasources) {
            if (datasource.DatasourceType == "SQL") {
              var datasourceId = datasource.DatasourceId;
              var gatewayId = datasource.GatewayId;
              // create credentials for Azure SQL database log in
              BasicCredentials creds = new BasicCredentials("CptStudent", "pass@word1");
              CredentialDetails details = new CredentialDetails();
              UpdateDatasourceRequest req = new UpdateDatasourceRequest();
              // Update credentials through gateway
              await pbiClient.Gateways.UpdateDatasourceAsync(gatewayId, datasourceId, req);
            }
          }
        }
      }
      return;
    }

    public static async Task PatchAnonymousDatasourceCredentials(string WorkspaceId, string importName) {
      PowerBIClient pbiClient = GetPowerBiClient();
      var datasets = (await pbiClient.Datasets.GetDatasetsInGroupAsync(WorkspaceId)).Value;
      foreach (var dataset in datasets) {
        if (importName.Equals(dataset.Name)) {
          string datasetId = dataset.Id;
          var datasources = (await pbiClient.Datasets.GetDatasourcesInGroupAsync(WorkspaceId, datasetId)).Value;
          foreach (var datasource in datasources) {
            if (datasource.DatasourceType == "OAuth" || datasource.DatasourceType == "File") {
              var datasourceId = datasource.DatasourceId;
              var gatewayId = datasource.GatewayId;
              // create credentials for Azure SQL database log in
              CredentialDetails details = new CredentialDetails("");
              UpdateDatasourceRequest req = new UpdateDatasourceRequest(details);
              // Update credentials through gateway
              await pbiClient.Gateways.UpdateDatasourceAsync(gatewayId, datasourceId, req);
            }
          }
        }
      }
      return;
    }
  }
}