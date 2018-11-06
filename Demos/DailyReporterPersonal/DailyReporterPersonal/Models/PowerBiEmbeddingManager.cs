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
using Newtonsoft.Json;

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
    private readonly static string appWorkspaceId = ConfigurationManager.AppSettings["app-workspace-id"];
    
    private static string GetAccessToken() {

      // create ADAL cache object
      string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
      ADALTokenCache userTokenCache = new ADALTokenCache();

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

    public static async Task<string> GetViewModelJSON() {
      PowerBIClient pbiClient = GetPowerBiClient();
      Object viewModel;
      if (appWorkspaceId == "") {
        viewModel = new {
          datasets = (await pbiClient.Datasets.GetDatasetsAsync()).Value,
          reports = (await pbiClient.Reports.GetReportsAsync()).Value,
          dashboards = (await pbiClient.Dashboards.GetDashboardsAsync()).Value,
          token = GetAccessToken()
        };
      }
      else {
        viewModel = new {
          datasets = (await pbiClient.Datasets.GetDatasetsInGroupAsync(appWorkspaceId)).Value,
          reports = (await pbiClient.Reports.GetReportsInGroupAsync(appWorkspaceId)).Value,
          dashboards = (await pbiClient.Dashboards.GetDashboardsInGroupAsync(appWorkspaceId)).Value,
          token = GetAccessToken()
        };
      }

      return JsonConvert.SerializeObject(viewModel);
    }

  }
}