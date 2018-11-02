using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldUsingRest.Models {

  public class Workspace {
    public string id { get; set; }
    public bool isReadOnly { get; set; }
    public string name { get; set; }
  }

  public class Dataset {
    public string id { get; set; }
    public string name { get; set; }
    public string webUrl { get; set; }
    public bool addRowsAPIEnabled { get; set; }
  }

  public class Report {
    public string id { get; set; }
    public int modelId { get; set; }
    public string name { get; set; }
    public string webUrl { get; set; }
    public string embedUrl { get; set; }
    public bool isOwnedByMe { get; set; }
    public bool isOriginalPbixReport { get; set; }
  }

  public class WorkspaceCollection {
    public List<Workspace> value { get; set; }
  }

  public class DatasetCollection {
    public List<Dataset> value { get; set; }
  }

  public class ReportCollection {
    public List<Report> value { get; set; }
  }
}
