using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using System.Security.Claims;
using System.Net;

namespace DailyReporterPersonal.Models {
  public class PowerBiEmbeddingManager {

    #region "private implementation details"

    private const string aadInstance = "https://login.microsoftonline.com/";

    private const string claimsIdentifierForTenantId = "http://schemas.microsoft.com/identity/claims/tenantid";
    private readonly static string tenantID = ClaimsPrincipal.Current.FindFirst(claimsIdentifierForTenantId).Value;
    private readonly static string tenantAuthority = aadInstance + tenantID;

    private const string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
    private const string urlPowerBiServiceApiRoot = "https://api.powerbi.com/";


    private readonly static string clientId = ConfigurationManager.AppSettings["client-id"];
    private readonly static string clientSecret = ConfigurationManager.AppSettings["client-secret"];
    private readonly static string replyUrl = ConfigurationManager.AppSettings["reply-url"];

    private static string GetAccessToken() {

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
        authenticationContext.AcquireTokenSilentAsync(
            resourceUriPowerBi,
            clientCredential,
            userIdentifier).Result;

      // return access token back to user
      return authenticationResult.AccessToken;

    }

    private static async Task<string> ExecuteGetRequest(string urlRestEndpoint) {

      string accessToken = GetAccessToken();

      HttpClient client = new HttpClient();
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, urlRestEndpoint);
      request.Headers.Add("Authorization", "Bearer " + accessToken);
      request.Headers.Add("Accept", "application/json;odata.metadata=minimal");

      HttpResponseMessage response = await client.SendAsync(request);

      if (response.StatusCode != HttpStatusCode.OK) {
        throw new ApplicationException("Error!!!!!");
      }

      return await response.Content.ReadAsStringAsync();
    }

    private static PowerBIClient GetPowerBiClient() {
      var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
      return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
    }


    #endregion

      public static ReportEmbeddingData GetReportEmbeddingData(string reportId) {

      PowerBIClient pbiClient = GetPowerBiClient();

      var report = pbiClient.Reports.GetReport(reportId);
      var embedUrl = report.EmbedUrl;
      var reportName = report.Name;
      var accessToken = GetAccessToken();

      return new ReportEmbeddingData {
        reportId = reportId,
        reportName = reportName,
        embedUrl = embedUrl,
        accessToken = accessToken
      };

    }

    public static DashboardEmbeddingData GetDashboardEmbeddingData(string dashboardId) {

      PowerBIClient pbiClient = GetPowerBiClient();

      var dashboard = pbiClient.Dashboards.GetDashboard(dashboardId);
      var embedUrl = dashboard.EmbedUrl;
      var dashboardDisplayName = dashboard.DisplayName;

      return new DashboardEmbeddingData {
        dashboardId = dashboardId,
        dashboardName = dashboardDisplayName,
        embedUrl = embedUrl,
        accessToken = GetAccessToken()
      };

    }

    public static DashboardTileEmbeddingData GetDashboardTileEmbeddingData(string dashboardId) {

      PowerBIClient pbiClient = GetPowerBiClient();

      var tiles = pbiClient.Dashboards.GetTiles(dashboardId).Value;

      // retrieve first tile in tiles connection
      var tile = tiles[0];
      var tileId = tile.Id;
      var tileTitle = tile.Title;
      var embedUrl = tile.EmbedUrl;

      return new DashboardTileEmbeddingData {
        dashboardId = dashboardId,
        TileId = tileId,
        TileTitle = tileTitle,
        embedUrl = embedUrl,
        accessToken = GetAccessToken()
      };

    }

    public static DatasetEmbeddingData GetDatasetEmbeddingData(string datasetId) {

      PowerBIClient pbiClient = GetPowerBiClient();

      return new DatasetEmbeddingData {
        datasetId = datasetId,
        embedUrlNewReport = "https://app.powerbi.com/reportEmbed",
        accessToken = GetAccessToken()
      };

    }

    public static async Task<ReportsViewModel> GetReports() {

      var client = GetPowerBiClient();
      var reports = (await client.Reports.GetReportsAsync()).Value;
      var reportsEmbeddingData = new List<ReportEmbeddingData>();
      foreach (var report in reports) {
        reportsEmbeddingData.Add(new ReportEmbeddingData {
          reportId = report.Id,
          reportName = report.Name,
          embedUrl = report.EmbedUrl,
          accessToken = GetAccessToken()
        });
      }


      var datasets = (await client.Datasets.GetDatasetsAsync()).Value;
      var datasetsEmbeddingData = new List<DatasetEmbeddingData>();
      foreach (var dataset in datasets) {
        datasetsEmbeddingData.Add(new DatasetEmbeddingData {
          datasetId = dataset.Id,
          datasetName = dataset.Name,
          embedUrlNewReport = "",
          embedUrlQnA = "",
          accessToken = GetAccessToken()
        });
      }


      var viewModel = new ReportsViewModel {
        Reports = reportsEmbeddingData,
        Datasets = datasetsEmbeddingData
      };

      return viewModel;


    }
  }
}