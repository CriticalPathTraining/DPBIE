using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyReporterPersonal.Models {

  // data required for embedding a report
  public class ReportEmbeddingData {
    public string reportId;
    public string reportName;
    public string embedUrl;
    public string accessToken;
  }

  // data required for embedding a dashboard
  public class DashboardEmbeddingData {
    public string dashboardId;
    public string dashboardName;
    public string embedUrl;
    public string accessToken;
  }

  // data required for embedding a dashboard
  public class DashboardTileEmbeddingData {
    public string dashboardId;
    public string TileId;
    public string TileTitle;
    public string embedUrl;
    public string accessToken;
  }

  // data required for embedding a new report
  public class DatasetEmbeddingData {
    public string datasetName;
    public string datasetId;
    public string embedUrlNewReport;
    public string embedUrlQnA;
    public string accessToken;
  }
  

}