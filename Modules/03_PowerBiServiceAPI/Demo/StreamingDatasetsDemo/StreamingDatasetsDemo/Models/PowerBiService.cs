using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

using Newtonsoft.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using StreamingDatasetsDemo.Models;

namespace StreamingDatasetsDemo {

  class PowerBiServiceWrapper {

    public const string AzureAuthorizationEndpoint = "https://login.microsoftonline.com/common";
    public const string PowerBiServiceResourceUri = "https://analysis.windows.net/powerbi/api";
    public const string PowerBiServiceRootUrl = "https://api.powerbi.com/v1.0/myorg/";
    public const string restUrlDatasets = PowerBiServiceRootUrl + "datasets";

    public const string ClientID = "[YOUR_CLIENT_ID]";
    public const string RedirectUri = "https://localhost/app1234";

    #region "Authentication Details"

    protected string AccessToken = string.Empty;

    protected void GetAccessToken() {

      // create new authentication context 
      var authenticationContext = new AuthenticationContext(AzureAuthorizationEndpoint);

      //// use authentication context to trigger user sign-in and return access token 
      var userAuthnResult = authenticationContext.AcquireTokenAsync(PowerBiServiceResourceUri,
                                                               ClientID,
                                                               new Uri(RedirectUri),
                                                               new PlatformParameters(PromptBehavior.Auto)).Result;

      // use authentication context to trigger user sign-in and return access token 
      //UserPasswordCredential creds = new UserPasswordCredential("MyAccount@MyDomain.onMicrosoft.com", "MyPassword");
      //var userAuthnResult = authenticationContext.AcquireTokenAsync(PowerBiServiceResourceUri,
      //                                                         ClientID,
      //                                                         creds).Result;


      // cache access token in AccessToken field
      AccessToken = userAuthnResult.AccessToken;

    }

    #endregion

    #region "REST operation utility methods"

    private string ExecuteGetRequest(string restUri) {

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

    private string ExecutePostRequest(string restUri, string postBody) {

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

    private string ExecuteDeleteRequest(string restUri) {
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

    public PowerBiServiceWrapper() {
      GetAccessToken();
    }

    private string GetDatasetId(string DatasetName) {
      string restUrlDatasets = PowerBiServiceRootUrl + "datasets/";
      string json = ExecuteGetRequest(restUrlDatasets);
      DatasetCollection datasets = JsonConvert.DeserializeObject<DatasetCollection>(json);
      foreach (var ds in datasets.value) {
        if (ds.name.Equals(DatasetName)) {
          return ds.id;
        }
      }
      return string.Empty;
    }

    public void CreateDemoStreamingDataset(string DatasetName) {
      ThreadStart ts = new ThreadStart(() => CreateDemoStreamingDatasetTask(DatasetName));
      Thread t = new Thread(ts);
      t.Start();
    }

    public void CreateDemoStreamingDatasetTask(string DatasetName) {

      // check to see if dataset exists
      string DatasetId = GetDatasetId(DatasetName);
      if (string.IsNullOrEmpty(DatasetId)) {
        Console.WriteLine("Creating new Dataset: " + DatasetName + "...");
        // create dataet if it does not exist
        string jsonNewDataset = Properties.Resources.DemoStreamingDataset.Replace("@DatasetName", DatasetName);
        // execute REST call to create new dataset
        string json = ExecutePostRequest(restUrlDatasets, jsonNewDataset);
        // retrieve Guid to track dataset ID
        Dataset dataset = JsonConvert.DeserializeObject<Dataset>(json);
        // get Dataset ID once it has been created
        DatasetId = GetDatasetId(DatasetName);
      }

      PushTemperatureRows(DatasetId);

    }

    public void CreateDemoHybridDataset(string DatasetName) {
      ThreadStart ts = new ThreadStart(() => CreateDemoHybridDatasetTask(DatasetName));
      Thread t = new Thread(ts);
      t.Start();
    }

    public void CreateDemoHybridDatasetTask(string DatasetName) {

      // check to see if dataset exists
      string DatasetId = GetDatasetId(DatasetName);
      if (string.IsNullOrEmpty(DatasetId)) {
        Console.WriteLine("Creating new Dataset: " + DatasetName + "...");
        // create dataet if it does not exist
        string jsonNewDataset = Properties.Resources.DemoHybridDataset.Replace("@DatasetName", DatasetName);
        // execute REST call to create new dataset
        string json = ExecutePostRequest(restUrlDatasets, jsonNewDataset);
        // retrieve Guid to track dataset ID
        Dataset dataset = JsonConvert.DeserializeObject<Dataset>(json);
        // get Dataset ID once it has been created
        DatasetId = GetDatasetId(DatasetName);
      }
      else {
        // if dataset exists, delete all existing table rows
        string restUrlTemperatureReadingsTableRows = string.Format("{0}/{1}/tables/TemperatureReadings/rows", restUrlDatasets, DatasetId);
        ExecuteDeleteRequest(restUrlTemperatureReadingsTableRows);
      }

      PushTemperatureRows(DatasetId);

    }

    public void PushTemperatureRows(string DatasetId) {
      int RunCount = 1;
      string RunName = "Run " + RunCount.ToString("00");
      double temperatureBatchA = 100;
      double temperatureBatchB = 100;
      double temperatureBatchC = 100;
      Random rand = new Random(714);

      Boolean tempOnTheRise = true;
      Boolean inTransition = false;
      int transitionCounter = 0;

      while (true) {

        if (inTransition) {
          transitionCounter += 1;
          int transitionCountMax = tempOnTheRise ? 15 : 3;
          if (transitionCounter >= transitionCountMax) {
            inTransition = false;
            transitionCounter = 0;
          }
        }
        else {
          if (tempOnTheRise) {
            temperatureBatchA += rand.Next(-40, 380) / (double)100;
            if (temperatureBatchA > 212) { temperatureBatchA = 212; }
            temperatureBatchB += rand.Next(0, 340) / (double)100;
            if (temperatureBatchB > 212) { temperatureBatchB = 212; }
            temperatureBatchC += rand.Next(20, 332) / (double)100;
            if (temperatureBatchC > 212) { temperatureBatchC = 212; }
            if (temperatureBatchA == 212 && temperatureBatchB == 212 && temperatureBatchC == 212) {
              tempOnTheRise = false;
              inTransition = true;
            }
          }
          else {
            temperatureBatchA -= rand.Next(0, 1020) / (double)100;
            if (temperatureBatchA < 100) { temperatureBatchA = 100; }
            temperatureBatchB -= rand.Next(100, 980) / (double)100;
            if (temperatureBatchB < 100) { temperatureBatchB = 100; }
            temperatureBatchC -= rand.Next(200, 1300) / (double)100;
            if (temperatureBatchC < 100) { temperatureBatchC = 100; }
            if (temperatureBatchA == 100 && temperatureBatchB == 100 && temperatureBatchC == 100) {
              tempOnTheRise = true;
              inTransition = true;
              RunCount += 1;
              RunName = "Run " + RunCount.ToString("00");
            }
          }
        }

        string currentTimeWindow = DateTime.Now.Hour.ToString("00") + ":" +
                                  DateTime.Now.Minute.ToString("00") + ":" +
                                  ((DateTime.Now.Second / 15) * 15).ToString("00");


        TemperatureReadingsRow row = new TemperatureReadingsRow {
          Run = RunName,
          Time = DateTime.Now,
          TimeWindow = currentTimeWindow,
          TargetTemperature = 212,
          MinTemperature = 100,
          MaxTemperature = 250,
          BatchA = temperatureBatchA,
          BatchB = temperatureBatchB,
          BatchC = temperatureBatchC,
        };

        TemperatureReadingsRow[] rows = { row };
        TemperatureReadingsRows temperatureReadingsRows = new TemperatureReadingsRows { rows = rows };
        string jsonNewRows = JsonConvert.SerializeObject(temperatureReadingsRows);
        string restUrlTargetTableRows = string.Format("{0}/{1}/tables/TemperatureReadings/rows", restUrlDatasets, DatasetId);
        string jsonResultAddExpenseRows = ExecutePostRequest(restUrlTargetTableRows, jsonNewRows);
        Console.Write(".");
        Thread.Sleep(500);
      }
    }

    public void CreateDemoPushDataset(string DatasetName) {
      ThreadStart ts = new ThreadStart(() => CreateDemoPushDatasetTask(DatasetName));
      Thread t = new Thread(ts);
      t.Start();
    }

    public void CreateDemoPushDatasetTask(string DatasetName) {
      string restUrlDatasets = PowerBiServiceRootUrl + "datasets";
      string restUrlContributionsTableRows;

      // check to see if dataset exists
      string DatasetId = GetDatasetId(DatasetName);
      if (string.IsNullOrEmpty(DatasetId)) {
        Console.WriteLine("Creating new Push Dataset named " + DatasetName + " ...");
        string jsonNewDataset = Properties.Resources.DemoPushDataset.Replace("@DatasetName", DatasetName);
        // execute REST call to create new dataset
        string json = ExecutePostRequest(restUrlDatasets, jsonNewDataset);
        // retrieve Guid to track dataset ID
        Dataset dataset = JsonConvert.DeserializeObject<Dataset>(json);
        DatasetId = GetDatasetId(DatasetName);
        restUrlContributionsTableRows = string.Format("{0}/{1}/tables/Contributions/rows", restUrlDatasets, DatasetId);
        // populate lookup tables
        PopulateHelperTables(DatasetId);
      }
      else {
        // if dataset exists, delete all existing table rows
        restUrlContributionsTableRows = string.Format("{0}/{1}/tables/Contributions/rows", restUrlDatasets, DatasetId);
        ExecuteDeleteRequest(restUrlContributionsTableRows);
      }

      while (true) {
        PushCampaignContributionRows(DatasetId);
        Thread.Sleep(3000);
        ExecuteDeleteRequest(restUrlContributionsTableRows);
        Thread.Sleep(5000);
      }
    }
  
    public void PopulateHelperTables(string DatasetId) {

      string jsonRowsGoal = @"{ ""rows"": [ { ""Value"": 1000000  }] }";
      string restUrlTableRowsGoal = string.Format("{0}/{1}/tables/ContributionsGoal/rows", restUrlDatasets, DatasetId);
      ExecuteDeleteRequest(restUrlTableRowsGoal);
      ExecutePostRequest(restUrlTableRowsGoal, jsonRowsGoal);

      string jsonRowsMax = @"{ ""rows"": [ { ""Value"": 1250000  }] }";
      string restUrlTableRowsMax = string.Format("{0}/{1}/tables/ContributionsMax/rows", restUrlDatasets, DatasetId);
      ExecuteDeleteRequest(restUrlTableRowsMax);
      ExecutePostRequest(restUrlTableRowsMax, jsonRowsMax);
    }

    public void PushCampaignContributionRows(string DatasetId) {
      int counter = 1;

      while (counter < 7) {
        AddRows(DatasetId);
        counter += 1;
        Thread.Sleep(1000);
      }


      while (counter < 21) {
        AddRows(DatasetId);
        counter += 1;
        Thread.Sleep(1000);
      }

      while (counter < 160) {
        AddRows(DatasetId);
        counter += 1;
        Thread.Sleep(500);
      }

    }

    public void AddRows(string DatasetId) {
      Console.Write(".");
      List<Contribution> contributionList = new List<Contribution>();
      foreach (var contribution in DataFactory.GetContributionList()) {
        contributionList.Add(new Contribution {
          ContributionID = contribution.ID,
          Contributor = contribution.FirstName + " " + contribution.LastName,
          City = contribution.City + ", " + contribution.State,
          State = contribution.State,
          Zipcode = contribution.Zipcode,
          Gender = contribution.Gender,
          Time = contribution.Time,
          TimeWindow = contribution.TimeWindow,
          Amount = contribution.Amount
        });
      }

      ContributionSet contributionSet = new ContributionSet {
        rows = contributionList.ToArray<Contribution>()
      };

      string jsonRows = JsonConvert.SerializeObject(contributionSet);

      string restUrlTableRows = string.Format("{0}/{1}/tables/Contributions/rows", restUrlDatasets, DatasetId);
      string json = ExecutePostRequest(restUrlTableRows, jsonRows);

    }
  }

  public class Dataset {
    public string id { get; set; }
    public string name { get; set; }
  }

  public class DatasetCollection {
    public List<Dataset> value { get; set; }
  }

  
  
}