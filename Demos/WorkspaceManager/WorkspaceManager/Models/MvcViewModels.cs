using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.PowerBI.Api.V2.Models;

namespace WorkspaceManager.Models {
  
  public class WorkspaceViewModel {
    public string Id { get; set; }
    public string Name { get; set; }
    public Group Workspace { get; set; }
    public IList<GroupUserAccessRight> workspaceUsers;
    public IList<Dataset> Datasets { get; set; }
    public IList<Report> Reports { get; set; }
    public IList<Dashboard> Dashboards { get; set; }
  }

  public class DatasetViewModel {
    public string WorkspaceId { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public Dataset Dataset { get; set; }
    public IList<Datasource> Datasources { get; set; }
    public IList<Refresh> RefreshHistroy { get; set; }
  }

  public class CapacityViewModel {
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public Capacity Capacity { get; set; }
    public IList<Group> Workspaces;
  }


}