using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyReporterPersonal.Models {

   public class HomeViewModel {
    public Boolean isUserAuthenticated;
  }

  public class ReportsViewModel {
    public List<ReportEmbeddingData> Reports { get; set; }
    public List<DatasetEmbeddingData> Datasets { get; set; }
  }

  public class DashboardViewModel {
  }

  public class DashboardsViewModel {
  }


}