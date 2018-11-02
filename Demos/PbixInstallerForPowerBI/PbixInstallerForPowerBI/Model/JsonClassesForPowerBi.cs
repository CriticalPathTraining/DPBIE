using System.Collections.Generic;


namespace PbixInstallerForPowerBI.Model {

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

  public class Import {
    public string id { get; set; }
    public string importState { get; set; }
    public string createdDateTime { get; set; }
    public string updatedDateTime { get; set; }
    public string name { get; set; }
    public string connectionType { get; set; }
    public string source { get; set; }
    public List<Dataset> datasets { get; set; }
    public List<Report> reports { get; set; }
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

  public class ImportCollection {
    public List<Import> value { get; set; }
  }

  public class Gateway {
    public string id { get; set; }
    public string gatewayId { get; set; }
    public string datasourceType { get; set; }
    public string connectionDetails { get; set; }
  }

  public class GatewayCollection{
    public List<Gateway> value { get; set; }
  }

  public class DataSourceCredentials {
    public string credentialType { get; set; }
    public BasicCredentials basicCredentials { get; set; }
  }

  public class BasicCredentials {
    public string username { get; set; }
    public string password { get; set; }
  }

}
