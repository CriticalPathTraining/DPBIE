
class Dashboard {
  id: string;
  displayName: string;
  embedUrl: string;
}

class Report {
  id: string;
  name: string;
  datasetId: string;
  embedUrl: string;
  webUrl: string;
}

class Dataset {
  id: string;
  name: string;
}

class ViewModel {
  dashboards: Dashboard[];
  reports: Report[];
  datasets: Dataset[];
  token: string;
}

$(() => {

  var dashboardsList = $("#dashboards-list");
  var reportsList = $("#reports-list");
  var datasetsList = $("#datasets-list");

  var viewModel: ViewModel = window['viewModel'];

  viewModel.dashboards.forEach((dashboard: Dashboard) => {
    var li = $("<li>");
    li.append($("<i>").addClass("fa fa-bar-chart"));
    li.append($("<a>", {
      "href": "javascript:void(0);"
    }).text(dashboard.displayName).click(() => { embedDashboard(dashboard) }));
    dashboardsList.append(li);
  });

  viewModel.reports.forEach((report: Report) => {
    var li = $("<li>");
    li.append($("<i>").addClass("fa fa-bar-chart"));
    li.append($("<a>", {
      "href": "javascript:void(0);"
    }).text(report.name).click(() => { embedReport(report) }));
    reportsList.append(li);
  });


  viewModel.datasets.forEach((dataset: Dataset) => {
    var li = $("<li>");
    li.append($("<i>").addClass("fa fa-bar-chart"));
    li.append($("<a>", {
      "href": "javascript:void(0);"
    }).text(dataset.name).click(() => { embedNewReport(dataset) }));
    datasetsList.append(li);
  });



});


var embedReport = (report: Report) => {

  var viewModel: ViewModel = window['viewModel'];
  var token: string = viewModel.token

  var models = window['powerbi-client'].models;

  var config = {
    type: 'report',
    id: report.id,
    embedUrl: report.embedUrl,
    accessToken: token,
    tokenType: models.TokenType.Aad,
    permissions: models.Permissions.All,
    viewMode: models.ViewMode.View,
    displayOptions: 1,
    settings: {
      filterPaneEnabled: false,
      navContentPaneEnabled: true
      //,background: models.BackgroundType.Transparent
    }
  };

  // Get a reference to the embedded report HTML element
  var reportContainer = document.getElementById('embed-container');

  var powerbi: any = window['powerbi'];

  // Embed the report and display it within the div container.
  powerbi.reset(reportContainer);
  var embeddedReport = powerbi.embed(reportContainer, config);


  window.location.hash = "report:" + report.id;


};


var embedDashboard = (dashboard: Dashboard) => {
  var viewModel: ViewModel = window['viewModel'];
  var token: string = viewModel.token

  var models = window['powerbi-client'].models;

  var config = {
    type: 'dashboard',
    id: dashboard.id,
    embedUrl: dashboard.embedUrl,
    accessToken: token,
    tokenType: models.TokenType.Aad,
    pageView: "fitToWidth" // options: "actualSize", "fitToWidth", "oneColumn"
  };
  
  // Get a reference to the embedded report HTML element
  var reportContainer = document.getElementById('embed-container');

  var powerbi: any = window['powerbi'];

  // Embed the report and display it within the div container.
  powerbi.reset(reportContainer);
  var embeddedReport = powerbi.embed(reportContainer, config);


  window.location.hash = "dashboard:" + dashboard.id;

}


var embedNewReport = (dataset: Dataset) => {

  var viewModel: ViewModel = window['viewModel'];
  var token: string = viewModel.token

  var models = window['powerbi-client'].models;

  var config = {
    datasetId: dataset.id,
    embedUrl: "https://app.powerbi.com/reportEmbed",
    accessToken: token,
    tokenType: models.TokenType.Aad
  };
  
  // Get a reference to the embedded report HTML element
  var reportContainer = document.getElementById('embed-container');

  var powerbi: any = window['powerbi'];

  // Embed the report and display it within the div container.
  powerbi.reset(reportContainer);
  var embeddedReport = powerbi.createReport(reportContainer, config);


  window.location.hash = "dataset:" + dataset.id;


};

window.onhashchange = function () {
 // this.console.log("Location: " + location.hash);
  
}
