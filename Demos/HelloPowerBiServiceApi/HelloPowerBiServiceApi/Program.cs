using System;
using System.Net;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using HelloPowerBiServiceApi.Models;

namespace HelloPowerBiServiceApi {

  class Program {

    const string aadAuthorizationEndpoint = "https://login.windows.net/common/oauth2/authorize";
    const string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
    const string clientId = "f4cd7acb-cfe7-4b49-a954-d8985dbfd9b1";
    static readonly Uri redirectUri = new Uri("https://localhost/app1234");

    static string GetAccessToken() {
      var authContext = new AuthenticationContext(aadAuthorizationEndpoint);
      var promptBehavior = new PlatformParameters(PromptBehavior.SelectAccount);
      AuthenticationResult authResult =
        authContext.AcquireTokenAsync(resourceUriPowerBi, clientId, redirectUri, promptBehavior).Result;
      return authResult.AccessToken;
    }

    static string GetAccessTokenUsingPasswordCredential() {
      var authContext = new AuthenticationContext(aadAuthorizationEndpoint);
      string userName = "user1@myorg.onMicrosoft.com";
      string userPassword = "pass@word1";
      var userCrednetials = new UserPasswordCredential(userName, userPassword);
      AuthenticationResult authResult = authContext.AcquireTokenAsync(resourceUriPowerBi, clientId, userCrednetials).Result;    
      return authResult.AccessToken;
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
      string restUrl = "https://api.powerbi.com/v1.0/myorg/reports/";
      var json = ExecuteGetRequest(restUrl);
      ReportCollection reports = JsonConvert.DeserializeObject<ReportCollection>(json);
      foreach (Report report in reports.value) {
        Console.WriteLine(report.name);
      }
    }

  }
}
